using JetBrains.ReSharper.Psi.Impl.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFmtStageContext : FormattingStageContext
  {
    public PsiFmtStageContext(FormattingRange range)
      : base(range)
    {
    }
  }
}