using System;
using System.Drawing;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Services;
using JetBrains.UI;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public class PsiDeclaredElementType : DeclaredElementType
  {
    private readonly IDeclaredElementPresenter myElementPresenter;
    private static readonly PsiLanguageType Language = PsiLanguage.Instance;

    private readonly string myImageName;

    public static readonly PsiDeclaredElementType Rule = new PsiDeclaredElementType("Rule", "psiRule.png");
    public static readonly PsiDeclaredElementType Role = new PsiDeclaredElementType("Role", "psiRole.png");
    public static readonly PsiDeclaredElementType Option = new PsiDeclaredElementType("Option", "psiOption.png");
    public static readonly PsiDeclaredElementType Variable = new PsiDeclaredElementType("Variable", "psiVariable.png");
    public static readonly PsiDeclaredElementType Path = new PsiDeclaredElementType("Path", "psiPath.png");

    private PsiDeclaredElementType(string name, String imageName)
      : base(name)
    {
      myElementPresenter = new PsiDeclaredElementPresenter();
      myImageName = imageName;
    }

    protected override Image GetImage(bool useVsIcons)
    {
      if (myImageName == null)
        return null;

      string prefix = string.Empty;
      if (useVsIcons)
        prefix = "vs.";

      return ImageLoader.GetImage("symbols." + prefix + myImageName, typeof(PsiDeclaredElementType).Assembly);
    }

    public override bool IsPresentable(PsiLanguageType language)
    {
      return (Equals(Language, language));
    }

    public override string PresentableName
    {
      get { return "Psi"; }
    }

    protected override IDeclaredElementPresenter DefaultPresenter
    {
      get { return myElementPresenter; }
    }
  }
}
