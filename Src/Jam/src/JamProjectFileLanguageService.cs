using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Jam.Resources;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.UI.Icons;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam
{
  [ProjectFileType(typeof (JamProjectFileType))]
  public class JamProjectFileLanguageService : ProjectFileLanguageService
  {
    public JamProjectFileLanguageService(JamProjectFileType projectFileType) : base(projectFileType) {}

    protected override PsiLanguageType PsiLanguageType
    {
      get { return JamLanguage.Instance; }
    }

    public override IconId Icon
    {
      get { return JamSymbolThemedIcons.File.Id; }
    }

    public override ILexerFactory GetMixedLexerFactory(ISolution solution, IBuffer buffer, IPsiSourceFile sourceFile = null)
    {
      var service = JamLanguage.Instance.LanguageService();
      Assertion.Assert(service != null, "service != null");
      return service.GetPrimaryLexerFactory();
    }
  }
}