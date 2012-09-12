using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Parsing;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Util
{
  internal class JamElementFactoryImpl : JamElementFactory
  {
    private readonly IPsiModule myModule;
    private readonly bool myApplyCodeFormatter;
    private readonly LanguageService myLanguageService;

    public JamElementFactoryImpl([NotNull] IPsiModule module, bool applyCodeFormatter)
    {
      if (module == null)
        throw new ArgumentNullException("module");

      myModule = module;
      myLanguageService = JamLanguage.Instance.LanguageService();
      myApplyCodeFormatter = applyCodeFormatter;
    }

    public override IJamFile CreateJamFile(string format, params object[] args)
    {
      if (format == null) throw new ArgumentNullException("format");
      return Create(parser => parser.ParseFile(), format, args);
    }

    public override T CreateDeclaration<T>(string format, params object[] args)
    {
      if (format == null) throw new ArgumentNullException("format");
      return (T) Create(parser => parser.ParseDeclaration(), format, args);
    }

    public override T CreateStatement<T>(string format, params object[] args)
    {
      if (format == null) throw new ArgumentNullException("format");
      return (T) Create(parser => parser.ParseStatement(), format, args);
    }

    public override T CreateExpression<T>(string format, params object[] args)
    {
      if (format == null) throw new ArgumentNullException("format");
      return (T) Create(parser => parser.ParseExpression(), format, args);
    }

    T Create<T>(Func<JamParser, T> parser, string format, params object[] args) where T : class, IJamTreeNode
    {
      var markers = ParseFormatString(ref format, args);

      var node = parser(CreateParser(myLanguageService.GetPrimaryLexerFactory().CreateLexer(new StringBuffer(format))));
      if (node == null || node.ContainsErrorElement())
        throw new ElementFactoryException(string.Format("Cannot create '{0}'", format));

      SandBox.CreateSandBoxFor(node, myModule);
      return (T)SubstituteNodes(node, markers, args);
    }

    private static ParameterMarker[] ParseFormatString(ref string format, params object[] args)
    {
      var markers = new List<ParameterMarker>();
      var sb = new StringBuilder();

      for (int i = 0; i < format.Length; )
      {
        if (format[i] == '$' && (i + 1 < format.Length) && char.IsDigit(format[i + 1]))
        {
          int paramNum = 0;
          for (i = i + 1; i < format.Length && char.IsDigit(format[i]); i++)
            paramNum = paramNum * 10 + int.Parse(format[i].ToString(CultureInfo.InvariantCulture));

          Assertion.Assert(paramNum >= 0 && paramNum < args.Length, "CreateElementFactory parameter number is out of range. ParamNum={0}, Args={1}, String={2}", paramNum, args.Length, format);
          var arg = args[paramNum];
          if (arg is string)
          {
            var str = (string)arg;
            sb.Append(str);
          }
          else if (arg is IStatement)
          {
            markers.Add(new ParameterMarker(new TreeTextRange(new TreeOffset(sb.Length), new TreeOffset(sb.Length + 1)), paramNum));
            sb.Append(";");
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



    private ITreeNode SubstituteNodes(ITreeNode root, ParameterMarker[] markers, object[] args)
    {
      Assertion.Assert(root.Parent is ISandBox, "root.Parent is IDummyHolder");

      if (myApplyCodeFormatter)
      {
        var service = JamLanguage.Instance.LanguageService();
        if (service != null)
        {
          var codeFormatter = service.CodeFormatter;
          if (codeFormatter != null) codeFormatter.Format(root, CodeFormatProfile.GENERATOR, null);
        }
      }

      var parent = root.Parent;

      // Finds the nodes to substitute
      var nodes = new ITreeNode[markers.Length];
      for (int i = 0; i < markers.Length; i++)
      {
        nodes[i] = root.FindNodeAt(markers[i].Range);
        if (nodes[i] == null)
          return null;
      }

      // if (myApplyCodeFormatter)
      // TODO: .FormatterInstance.Format(root, CodeFormatProfile.GENERATOR, null);

      // Do substitution
      for (int i = 0; i < markers.Length; i++)
      {
        object arg = args[markers[i].ParamNumber];

        if (arg is ITreeNode)
        {
          Assertion.Assert(((ITreeNode)arg).IsValid(), "((ITreeNode)arg).IsValid()");
          ITreeNode node = FindNodeAtRangeByType(nodes[i], arg.GetType());
          if (node == null)
            return null;
          ModificationUtil.ReplaceChild(node, ((ITreeNode)arg));
        }
        else
          throw new InvalidOperationException(String.Format("Unexpected operand type {0}", arg.GetType()));
      }

      return parent.FirstChild;
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

    private JamParser CreateParser(ILexer lexer)
    {
      return (JamParser)myLanguageService.CreateParser(lexer, myModule, null);
    }
  }
}