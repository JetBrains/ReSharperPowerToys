using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  public class PsiElementFactoryImpl : PsiElementFactory
  {
    private readonly PsiLanguageService myLanguageService;
    private readonly IPsiModule myModule;

    public PsiElementFactoryImpl([NotNull] IPsiModule module)
      : this(module, module.GetSolution())
    {
    }

    private PsiElementFactoryImpl([NotNull] IPsiModule module, [NotNull] ISolution solution)
    {
      myModule = module;
      Solution = solution;
      Solution.GetPsiServices();
      myLanguageService = (PsiLanguageService)PsiLanguage.Instance.LanguageService();
    }

    private PsiParser CreateParser(string text)
    {
      return (PsiParser)myLanguageService.CreateParser(myLanguageService.GetPrimaryLexerFactory().CreateLexer(new StringBuffer(text)), null, null);
    }

    public override IRuleName CreateIdentifierExpression(string name)
    {
      var expression = (IRuleName)CreateExpression("$0", name);
      return expression;
    }

    public override IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters = false)
    {
      string braceParameters = "";
      if(hasBraceParameters)
      {
        braceParameters = " {ROLE, getter} ";
      }
      var node = CreateParser(name + braceParameters + "\n" + ":" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name + braceParameters + "\n" + ":" + "\n" + ";"));
      }
      SandBox.CreateSandBoxFor(node, myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name));
    }

    public override IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters, IList<Pair<string, string>> variableParameters)
    {
      if(variableParameters.Count == 0)
      {
        return CreateRuleDeclaration(name, hasBraceParameters);
      }

      string braceParameters = "";
      if (hasBraceParameters)
      {
        braceParameters = " {ROLE, getter} ";
      }

      string variableParametersString = " [";
      foreach (var variableParameter in variableParameters)
      {
        variableParametersString = variableParametersString + variableParameter.Second + " " + variableParameter.First + ",";
      }
      variableParametersString = variableParametersString.Substring(0, variableParametersString.Length - 1) + "]";

      var node = CreateParser(name + braceParameters + variableParametersString + "\n" + ":" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name + braceParameters + variableParametersString + "\n" + ":" + "\n" + ";"));
      }
      SandBox.CreateSandBoxFor(node, myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name));
    }

    private ITreeNode CreateExpression(string format, string name)
    {
      var node = CreateParser(name + "\n" + ":" + name + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
      }
      SandBox.CreateSandBoxFor(node, myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        IRuleBody ruleBody = ruleDeclaration.Body;
        ITreeNode child = ruleBody.FirstChild;
        while (child != null && ! (child is IPsiExpression))
        {
          child = child.NextSibling;
        }
        while (child != null && !(child is IRuleName))
        {
          child = child.FirstChild;
        }
        if (child != null)
        {
          return child;
        }
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'" + name, format));
    }
  }
}
