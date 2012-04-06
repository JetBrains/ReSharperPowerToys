using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiCodeFormattingSettings
  {
    public readonly IContextBoundSettingsStore ContextBoundSettingsStore;
    public readonly ISettingsOptimization SettingsOptimization;
    public readonly GlobalFormatSettings GlobalSettings;

    public PsiCodeFormattingSettings(IContextBoundSettingsStore contextBoundSettingsStore, ISettingsOptimization settingsOptimization, GlobalFormatSettings globalSettings)
    {
      ContextBoundSettingsStore = contextBoundSettingsStore;
      SettingsOptimization = settingsOptimization;
      GlobalSettings = globalSettings;
    }
  }
}