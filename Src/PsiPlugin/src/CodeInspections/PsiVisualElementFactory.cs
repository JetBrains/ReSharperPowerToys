using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  [Language(typeof(PsiLanguage))]
  internal class PsiVisualElementFactory : IVisualElementFactory
  {
    public IColorReference GetColorReference(ITreeNode element)
    {
      return null;
    }
  }
}
