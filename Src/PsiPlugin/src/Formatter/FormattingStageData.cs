using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class FormattingStageData
  {
    public FormattingStageData(PsiCodeFormattingSettings formattingSettings, CodeFormattingContext context, PsiFormatProfile profile, List<IPsiCodeFormatterExtension> extensions)
    {
      FormattingSettings = formattingSettings;
      Extensions = extensions;
      Context = context;
      Profile = profile;
    }

    public PsiCodeFormattingSettings FormattingSettings { get; private set; }
    public List<IPsiCodeFormatterExtension> Extensions { get; private set; }
    public CodeFormattingContext Context { get; private set; }
    public PsiFormatProfile Profile { get; private set; }
  }
}
