using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace JetBrains.ReSharper.LexPlugin.Completion
{
  [SolutionComponent]
  public class LexIntellisenseManager : LanguageSpecificIntellisenseManager
  {
    public LexIntellisenseManager(ISettingsStore settingsStore)
      : base(settingsStore)
    {
      SettingsStore = settingsStore;
    }

    protected override bool GetIntellisenseEnabledSpecific(IContextBoundSettingsStore boundSettingsStore)
    {
      return boundSettingsStore.GetValue((IntellisenseEnabledSettingPsi setting) => setting.IntellisenseEnabled);
    }
  }

  [SettingsKey(typeof (AutopopupEnabledSettingsKey), "Lex")]
  public class LexAutopopupEnabledSettingsKey
  {
    [SettingsEntry(AutopopupType.HardAutopopup, "After dot")]
    public AutopopupType AfterDot;

    [SettingsEntry(AutopopupType.HardAutopopup, "On letters and digits")]
    public AutopopupType OnIdent;
  }

  [SettingsKey(typeof (IntellisenseEnabledSettingsKey), "Override VS IntelliSense for Lex")]
  public class IntellisenseEnabledSettingPsi
  {
    [SettingsEntry(false, "Lex (.lex files and embedded Lex)")]
    public bool IntellisenseEnabled;
  }
}
