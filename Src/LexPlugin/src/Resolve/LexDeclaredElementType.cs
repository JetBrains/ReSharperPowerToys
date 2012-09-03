using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Services;
using JetBrains.ReSharper.Psi;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.LexPlugin.Resolve
{
  public class LexDeclaredElementType : DeclaredElementType
  {
    private static readonly PsiLanguageType Language = LexLanguage.Instance;

    public static readonly LexDeclaredElementType Token = new LexDeclaredElementType("Rule");
    private readonly IDeclaredElementPresenter myElementPresenter;
    private readonly IconId myIconId;

    public LexDeclaredElementType(string name)
      : base(name)
    {
      myElementPresenter = new LexDeclaredElementPresenter();
      myIconId = null;
    }

    #region Overrides of DeclaredElementType

    protected override IconId GetImage()
    {
      return myIconId;
    }

    public override bool IsPresentable(PsiLanguageType language)
    {
      return (Equals(Language, language));
    }

    public override string PresentableName
    {
      get { return "Lex"; }
    }

    protected override IDeclaredElementPresenter DefaultPresenter
    {
      get { return myElementPresenter; }
    }

    #endregion
  }
}
