using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Lexer;
using JetBrains.ReSharper.PsiPlugin.Lexer.Psi;
using JetBrains.ReSharper.PsiPlugin.Resources;
using JetBrains.Text;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.PsiPlugin.PsiGrammar
{
  [ProjectFileType(typeof (PsiProjectFileType))]
  public class PsiProjectFileLanguageService : ProjectFileLanguageService
  {
    public PsiProjectFileLanguageService(PsiProjectFileType projectFileType)
      : base(projectFileType)
    {
    }

    protected override PsiLanguageType PsiLanguageType
    {
      get { return PsiLanguage.Instance; }
    }

    public override IconId Icon
    {
      get { return PsiPluginSymbolThemedIcons.PsiFile.Id; }
    }

    public override ILexerFactory GetMixedLexerFactory(ISolution solution, IBuffer buffer, IPsiSourceFile sourceFile = null)
    {
      {
        return new PsiLexerFactory();
      }
    }
  }
}
