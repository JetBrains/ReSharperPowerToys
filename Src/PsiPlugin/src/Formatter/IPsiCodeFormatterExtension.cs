using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public interface IPsiCodeFormatterExtension
  {
    bool? FormatSingleLine(ITreeNode context);
  }
}