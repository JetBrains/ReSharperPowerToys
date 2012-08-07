using JetBrains.ReSharper.Psi.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiCodeFormattingSettings
  {
    public PsiCodeFormattingSettings(GlobalFormatSettings globalSettings)
    {
      GlobalSettings = globalSettings;
      //CommonSettings = commonSettings;
    }

    //public readonly CommonFormatterSettingsKey CommonSettings;
    public readonly GlobalFormatSettings GlobalSettings;
  }
}
