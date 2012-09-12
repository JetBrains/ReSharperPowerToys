using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  internal interface IPsiCodeFormatterFactory
  {
    [NotNull]
    PsiFormattingVisitor CreateFormattingVisitor([NotNull] CodeFormattingContext formattingData);
  }
}
