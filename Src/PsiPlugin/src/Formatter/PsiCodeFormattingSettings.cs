using JetBrains.ReSharper.Psi.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiCodeFormattingSettings
  {
    public PsiCodeFormattingSettings(GlobalFormatSettings globalSettings, CommonFormatterSettingsKey commonSettings)
    {
      GlobalSettings = globalSettings;
      CommonSettings = commonSettings;
    }

    public readonly CommonFormatterSettingsKey CommonSettings;
    public readonly GlobalFormatSettings GlobalSettings;
  }
}
