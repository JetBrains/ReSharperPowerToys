using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  class PsiElementFactoryImpl : PsiElementFactory
  {
    private readonly bool myApplyCodeFormatter = true;
    private readonly PsiLanguageService myLanguageService;
    private readonly IPsiModule myModule;

    public PsiElementFactoryImpl([NotNull] IPsiModule module, bool applyCodeFormatter)
      : this(module, module.GetSolution(), applyCodeFormatter)
    {
    }

    private PsiElementFactoryImpl([NotNull] IPsiModule module, [NotNull] ISolution solution, bool applyCodeFormatter)
    {
      myModule = module;
      Solution = solution;
      Solution.GetPsiServices();
      myLanguageService = (PsiLanguageService)PsiLanguage.Instance.LanguageService();
      myApplyCodeFormatter = applyCodeFormatter;
    }

    private PsiParser CreateParser(string text)
    {
      return (PsiParser)myLanguageService.CreateParser(myLanguageService.GetPrimaryLexerFactory().CreateLexer(new StringBuffer(text)), null, null);
    }

    private static ParameterMarker[] ParseFormatString(ref string format, params object[] args)
    {
      var markers = new List<ParameterMarker>();
      var sb = new StringBuilder();


      for (int i=0; i<format.Length;)
      {
        if (format[i] == '$' && (i + 1 < format.Length) && char.IsDigit(format[i+1]))
        {
          int paramNum = 0;
          for (i = i + 1; i < format.Length && char.IsDigit(format[i]); i++)
            paramNum = paramNum*10 + int.Parse(format[i].ToString());

          Assertion.Assert(paramNum >= 0 && paramNum < args.Length, string.Format("CreateElementFactory parameter number is out of range. ParamNum={0}, Args={1}, String={2}", paramNum, args.Length, format));
          var arg = args[paramNum];
          if (arg is string)
          {
            var str = (string)arg;
            sb.Append(str);
          }
          else if (arg is IPsiStatement)
          {
            markers.Add(new ParameterMarker(new TreeTextRange(new TreeOffset(sb.Length), new TreeOffset(sb.Length + 1)), paramNum));
            sb.Append(";");
          }
          else if (arg is IType)
          {
            markers.Add(new ParameterMarker(new TreeTextRange(new TreeOffset(sb.Length), new TreeOffset(sb.Length + 1)), paramNum));
            sb.Append("A");
          }
          else if (arg is IDeclaredElement && !(arg is IExpression))
          {
            var name = ((IDeclaredElement)arg).ShortName;
            markers.Add(new ParameterMarker(new TreeTextRange(new TreeOffset(sb.Length), new TreeOffset(sb.Length + name.Length)), paramNum));
            sb.Append(name);
          }
          else if (arg is DeclaredElementInstance)
          {
            string name = ((DeclaredElementInstance)arg).Element.ShortName;
            markers.Add(new ParameterMarker(new TreeTextRange(new TreeOffset(sb.Length), new TreeOffset(sb.Length + name.Length)), paramNum));
            sb.Append(name);
          }
          else
          {
            markers.Add(new ParameterMarker(new TreeTextRange(new TreeOffset(sb.Length), new TreeOffset(sb.Length + 1)), paramNum));
            sb.Append("A");
          }

          continue;
        }

        sb.Append(format[i++]);
      }

      format = sb.ToString();
      return markers.ToArray();
    }

    private static ITreeNode FindNodeAtRangeByType(ITreeNode node, Type type)
    {
      TreeTextRange range = node.GetTreeTextRange();
      ITreeNode foundNode = null;
      while (node != null && node.GetTreeTextRange() == range)
      {
        if (type.IsInstanceOfType(node))
          foundNode = node;
        node = node.Parent;
      }
      return foundNode;
    }

    private ITreeNode SubstituteNodes(ITreeNode root, ParameterMarker[] markers, object[] args)
    {
      Assertion.Assert(root.Parent is ISandBox, "root.Parent is IDummyHolder");
      var parent = root.Parent;

      // Finds the nodes to substitute
      var nodes = new ITreeNode[markers.Length];
      for (int i = 0; i < markers.Length; i++)
      {
        nodes[i] = root.FindNodeAt(markers[i].Range);
        if (nodes[i] == null)
          return null;
      }

      if (myApplyCodeFormatter)
        root.Language.LanguageService().CodeFormatter.Format(root, CodeFormatProfile.GENERATOR, null);

      // Do substitution
      for (int i = 0; i < markers.Length; i++)
      {
        object arg = args[markers[i].ParamNumber];

        if (arg is IDeclaredElement && !(arg is IPsiExpression))
        {
          IQualifiableReference qualifiableReference = null;
          TreeTextRange range = nodes[i].GetTreeTextRange();
          IReference[] references = parent.FindReferencesAt(range);
          foreach (IReference reference in references)
          {
            if (reference.GetTreeTextRange() != range)
              continue;
            qualifiableReference = reference as IQualifiableReference;
            if (qualifiableReference != null)
              break;
          }

          if (qualifiableReference == null)
            return null;
          Assertion.Assert(((IDeclaredElement)arg).IsValid(), "((IDeclaredElement)arg).IsValid()");
          qualifiableReference.BindTo((IDeclaredElement) arg);
        }
        else if (arg is IPsiExpression)
        {
          Assertion.Assert(((ITreeNode)arg).IsValid(), "((ITreeNode)arg).IsValid()");
          ITreeNode node = FindNodeAtRangeByType(nodes[i], typeof(IPsiExpression));
          if (node == null)
            return null;
          //((IPsiExpression) node).ReplaceBy((IPsiExpression) arg);
        }
        else if (arg is IPsiStatement)
        {
          Assertion.Assert(((ITreeNode)arg).IsValid(), "((ITreeNode)arg).IsValid()");
          ITreeNode node = FindNodeAtRangeByType(nodes[i], typeof(IPsiStatement));
          if (node == null)
            return null;
          ((IPsiStatement)node).ReplaceBy((IPsiStatement)arg);
        }
        else if (arg is ITreeNode)
        {
          Assertion.Assert(((ITreeNode)arg).IsValid(), "((ITreeNode)arg).IsValid()");
          ITreeNode node = FindNodeAtRangeByType(nodes[i], arg.GetType());
          if (node == null)
            return null;
          ModificationUtil.ReplaceChild(node, ((ITreeNode)arg));
        } else
          throw new InvalidOperationException(String.Format("Unexpected operand type {0}", arg.GetType()));
      }

      return parent.FirstChild;
    }

    #region IElementFactory members

    protected IPsiFile CreateFileImpl(string text, params object[] args)
    {
      ParameterMarker[] markers = null;
      if (args != null && args.Length > 0)
        markers = ParseFormatString(ref text, args);

      var buffer = new StringBuffer(text);
      var lexer = myLanguageService.GetPrimaryLexerFactory().CreateLexer(buffer);
      var tokenBuffer = new TokenBuffer(lexer);

      IParser parser = myLanguageService.CreateParser(tokenBuffer.CreateLexer(), myModule, null);
      IFile file = parser.ParseFile();
      if (file == null)
        return null;

      IFile node = file;
      SandBox.CreateSandBoxFor(node, myModule);

      if (markers != null)
        node = (IFile) SubstituteNodes(node, markers, args);
      else if (myApplyCodeFormatter)
        myLanguageService.CodeFormatter.Format(node, CodeFormatProfile.GENERATOR, null);

      return (IPsiFile)node;
    }

    #endregion

    #region Nested type: ParameterMarker

    private struct ParameterMarker
    {
      public readonly int ParamNumber;
      public readonly TreeTextRange Range;

      public ParameterMarker(TreeTextRange range, int paramNumber)
      {
        Range = range;
        ParamNumber = paramNumber;
      }
    }

    #endregion

    public override IRuleName CreateIdentifierExpression(string name)
    {
      var expression = (IRuleName)CreateExpression("$0", name);
      return expression;
    }

    private ITreeNode CreateExpression(string format, string name)
    {
      var node = CreateParser(name + "\n" + ":" + "token" + "\n" + ";").parsePsiFile(false) as IPsiFile;
      if (node == null)
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
      SandBox.CreateSandBoxFor(node, myModule);
      IRuleDeclaration ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if(ruleDeclaration != null)
      {
        return ruleDeclaration.RuleName;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
    }
  }
}
