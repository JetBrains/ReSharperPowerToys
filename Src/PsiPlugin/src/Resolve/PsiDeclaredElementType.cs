using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Resources;
using JetBrains.ReSharper.PsiPlugin.Services;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiDeclaredElementType : DeclaredElementType
  {
    private static readonly PsiLanguageType Language = PsiLanguage.Instance;

    public static readonly PsiDeclaredElementType Rule = new PsiDeclaredElementType("Rule", PsiPluginSymbolThemedIcons.PsiRule.Id);
    public static readonly PsiDeclaredElementType Role = new PsiDeclaredElementType("Role", PsiPluginSymbolThemedIcons.PsiRole.Id);
    public static readonly PsiDeclaredElementType Option = new PsiDeclaredElementType("Option", PsiPluginSymbolThemedIcons.PsiOption.Id);
    public static readonly PsiDeclaredElementType Variable = new PsiDeclaredElementType("Variable", PsiPluginSymbolThemedIcons.PsiVariable.Id);
    public static readonly PsiDeclaredElementType Path = new PsiDeclaredElementType("Path", PsiPluginSymbolThemedIcons.PsiPath.Id);
    private readonly IDeclaredElementPresenter myElementPresenter;
    private readonly IconId myIconId;

    private PsiDeclaredElementType(string name, IconId iconId)
      : base(name)
    {
      myElementPresenter = new PsiDeclaredElementPresenter();
      myIconId = iconId;
    }

    public override string PresentableName
    {
      get { return "Psi"; }
    }

    protected override IDeclaredElementPresenter DefaultPresenter
    {
      get { return myElementPresenter; }
    }

    protected override IconId GetImage()
    {
      return myIconId;
    }

    public override bool IsPresentable(PsiLanguageType language)
    {
      return (Equals(Language, language));
    }
  }
}
