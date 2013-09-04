using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.LexPlugin.Completion
{
  [SolutionComponent]
  public class LexAutomaticStrategy : IAutomaticCodeCompletionStrategy
  {
    private readonly LexIntellisenseManager myPsiIntellisenseManager;
    private readonly SettingsScalarEntry mySettingsScalarEntry;

    public LexAutomaticStrategy(LexIntellisenseManager psiIntellisenseManager, ISettingsStore settingsStore)
    {
      myPsiIntellisenseManager = psiIntellisenseManager;
      mySettingsScalarEntry = settingsStore.Schema.GetScalarEntry((LexAutopopupEnabledSettingsKey key) => key.OnIdent);
    }

    public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
    {
      return (AutopopupType) settingsStore.GetValue(mySettingsScalarEntry, null);
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
      //return textControl.Caret.Offset() == 1 || this.MatchText(textControl, 1, text => !IsIdentStart(text[0]));
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
      get { return LexLanguage.Instance; }
    }

    private static bool IsIdentStart(char c)
    {
      return char.IsLetter(c);
    }
  }
}
