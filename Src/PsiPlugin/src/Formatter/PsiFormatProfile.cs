using JetBrains.ReSharper.Psi.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormatProfile
  {
    public PsiFormatProfile(CodeFormatProfile profile)
    {
      Profile = profile;
    }

    public CodeFormatProfile Profile { get; set; }
  }
}