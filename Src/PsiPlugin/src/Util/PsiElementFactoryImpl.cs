using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  class PsiElementFactoryImpl : PsiElementFactory
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

    #region IElementFactory members

    #endregion

    #region Nested type: ParameterMarker

    #endregion

    public override IRuleName CreateIdentifierExpression(string name)
    {
      var expression = (IRuleName)CreateExpression("$0", name);
      return expression;
    }

    private ITreeNode CreateExpression(string format, string name)
    {
      var node = CreateParser(name + "\n" + ":" + "token" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
      SandBox.CreateSandBoxFor(node, myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if(ruleDeclaration != null)
      {
        return ruleDeclaration.RuleName;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
    }
  }
}
