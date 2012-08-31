using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi
{
  [Language(typeof (PsiLanguage))]
  internal class PsiVisualElementFactory : IVisualElementFactory
  {
    #region IVisualElementFactory Members

    public IColorReference GetColorReference(ITreeNode element)
    {
      return null;
    }

    #endregion
  }
}
