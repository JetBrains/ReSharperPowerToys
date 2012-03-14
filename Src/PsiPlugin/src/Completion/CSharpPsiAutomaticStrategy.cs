using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Settings;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [SolutionComponent]
  public class CSharpPsiAutomaticStrategy : IAutomaticCodeCompletionStrategy
  {
    private readonly CSharpIntellisenseManager myCSharpIntellisenseManager;
    private readonly SettingsScalarEntry mySettingsScalarEntry;

    public CSharpPsiAutomaticStrategy(CSharpIntellisenseManager cSharpIntellisenseManager, ISettingsStore settingsStore)
    {
      myCSharpIntellisenseManager = cSharpIntellisenseManager;
      mySettingsScalarEntry = settingsStore.Schema.GetScalarEntry((PsiAutopopupEnabledSettingsKey key) => key.OnIdent);
    }

    public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
    {
      return (AutopopupType) settingsStore.GetValue(mySettingsScalarEntry, null);
    }

    public bool AcceptTyping(char c, ITextControl textControl, IContextBoundSettingsStore boundSettingsStore)
    {
      if (!IsIdentStart(c))
        return false;

      if (!myCSharpIntellisenseManager.GetAutoppopupEnabled(boundSettingsStore))
        return false;
      return true;
      //return textControl.Caret.Offset() == 1 || this.MatchText(textControl, 1, text => !IsIdentStart(text[0]));
    }

    private static bool IsIdentStart(char c)
    {
      return char.IsLetter(c);
    }

    public bool ProcessSubsequentTyping(char c, ITextControl textControl)
    {
      return (IsIdentStart(c) || char.IsDigit(c)|| c == '_');
    }

    public bool AcceptsFile(IFile file, ITextControl textControl)
    {
      return this.MatchTokenType(file, textControl, token => token.IsIdentifier || token.IsKeyword);
    }

    public CodeCompletionType CodeCompletionType
    {
      get 
      {
        return CodeCompletionType.AutomaticCompletion;
      }
    }

    public PsiLanguageType Language
    {
      get
      {
        return CSharpLanguage.Instance;
      }
    }

  }
}