using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  [ProjectFileType(typeof (PsiProjectFileType))]
  public class PsiCodeFormatterFactory : IPsiCodeFormatterFactory
  {
    #region IPsiCodeFormatterFactory Members

    public PsiFormattingVisitor CreateFormattingVisitor(CodeFormattingContext context)
    {
      return new PsiFormattingVisitor(context);
    }

    #endregion
  }
}
