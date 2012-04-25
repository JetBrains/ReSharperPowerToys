using JetBrains.ReSharper.Psi.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiCodeFormattingSettings
  {
    public readonly GlobalFormatSettings GlobalSettings;

    public PsiCodeFormattingSettings(GlobalFormatSettings globalSettings)
    {
      GlobalSettings = globalSettings;
    }
  }
}