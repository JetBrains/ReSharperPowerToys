using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Settings;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  //[SolutionComponent]
  public class CSharpPsiOnPunctuatorStrategy : IAutomaticCodeCompletionStrategy
  {
    private readonly CSharpIntellisenseManager myCSharpIntellisenseManager;
    private readonly SettingsScalarEntry mySettingsScalarEntry;

    public CSharpPsiOnPunctuatorStrategy(CSharpIntellisenseManager cSharpIntellisenseManager, ISettingsStore settingsStore)
    {
      myCSharpIntellisenseManager = cSharpIntellisenseManager;
      mySettingsScalarEntry = settingsStore.Schema.GetScalarEntry((CSharpAutopopupEnabledSettingsKey key) => key.AfterDot);
    }

    public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
    {
      return (AutopopupType) settingsStore.GetValue( mySettingsScalarEntry, null);
    }

    public bool AcceptTyping(char c, ITextControl textControl, IContextBoundSettingsStore boundSettingsStore)
    {
      if (c != '.' && c != ':' && c != '#')
      {
        return false;
      }

      if (!myCSharpIntellisenseManager.GetAutoppopupEnabled(boundSettingsStore))
        return false;

      return true;
    }

    public bool ProcessSubsequentTyping(char c, ITextControl textControl)
    {
      return char.IsLetterOrDigit(c);
    }

    public bool AcceptsFile(IFile file, ITextControl textControl)
    {
      return this.MatchTokenType(file, textControl, type => type.IsIdentifier || type.IsKeyword || type == CSharpTokenType.DOT || type == CSharpTokenType.COLON || type == CSharpTokenType.DOUBLE_COLON);
    }

    public CodeCompletionType CodeCompletionType
    {
      get {
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
