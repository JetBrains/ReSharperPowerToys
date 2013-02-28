using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [SolutionComponent]
  public class PsiAutomaticStrategy : IAutomaticCodeCompletionStrategy
  {
    private readonly PsiIntellisenseManager myPsiIntellisenseManager;
    private readonly SettingsScalarEntry mySettingsScalarEntry;

    public PsiAutomaticStrategy(PsiIntellisenseManager psiIntellisenseManager, ISettingsStore settingsStore)
    {
      myPsiIntellisenseManager = psiIntellisenseManager;
      mySettingsScalarEntry = settingsStore.Schema.GetScalarEntry((PsiAutopopupEnabledSettingsKey key) => key.OnIdent);
    }

    #region IAutomaticCodeCompletionStrategy Members

    public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
    {
      return (AutopopupType)settingsStore.GetValue(mySettingsScalarEntry, null);
    }

    public bool AcceptTyping(char c, ITextControl textControl, IContextBoundSettingsStore boundSettingsStore)
    {
      if (!IsIdentStart(c))
      {
        return false;
      }

      if (!myPsiIntellisenseManager.GetAutoppopupEnabled(boundSettingsStore))
      {
        return false;
      }
      return true;
    }

    public bool ProcessSubsequentTyping(char c, ITextControl textControl)
    {
      return (IsIdentStart(c) || char.IsDigit(c) || c == '_');
    }

    public bool AcceptsFile(IFile file, ITextControl textControl)
    {
      return this.MatchTokenType(file, textControl, token => token.IsIdentifier || token.IsKeyword);
    }

    public CodeCompletionType CodeCompletionType
    {
      get { return CodeCompletionType.AutomaticCompletion; }
    }

    public bool ForceHideCompletion
    {
      get { return false; }
    }

    public PsiLanguageType Language
    {
      get { return PsiLanguage.Instance; }
    }

    #endregion

    private static bool IsIdentStart(char c)
    {
      return char.IsLetter(c);
    }
  }
}
