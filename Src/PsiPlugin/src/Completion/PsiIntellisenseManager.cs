using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [SolutionComponent]
  public class PsiIntellisenseManager : LanguageSpecificIntellisenseManager
  {
    public PsiIntellisenseManager(ISettingsStore settingsStore)
      : base(settingsStore)
    {
      SettingsStore = settingsStore;
    }

    protected override bool GetIntellisenseEnabledSpecific(IContextBoundSettingsStore boundSettingsStore)
    {
      return boundSettingsStore.GetValue((IntellisenseEnabledSettingPsi setting) => setting.IntellisenseEnabled);
    }
  }

  [SettingsKey(typeof (AutopopupEnabledSettingsKey), "Psi")]
  public class PsiAutopopupEnabledSettingsKey
  {
    [SettingsEntry(AutopopupType.HardAutopopup, "After dot")]
    public AutopopupType AfterDot;

    [SettingsEntry(AutopopupType.HardAutopopup, "On letters and digits")]
    public AutopopupType OnIdent;
  }

  [SettingsKey(typeof (IntellisenseEnabledSettingsKey), "Override VS IntelliSense for Psi")]
  public class IntellisenseEnabledSettingPsi
  {
    [SettingsEntry(false, "Psi (.psi files and embedded Psi)")]
    public bool IntellisenseEnabled;
  }
}
