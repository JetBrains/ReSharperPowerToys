using JetBrains.Annotations;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  internal interface IPsiCodeFormatterFactory
  {
    [NotNull]
    PsiFormattingVisitor CreateFormattingVisitor([NotNull] FormattingStageData formattingData);
  }
}