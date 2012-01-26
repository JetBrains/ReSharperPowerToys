using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.VisualElements;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util.Colors;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  [Language(typeof (PsiLanguage))]
  internal class VisualElementFactory : IVisualElementFactory
  {
    public IColorReference GetColorReference(ITreeNode element)
    {
      var reference = element as IReference;
      if(reference == null)
      {
        return null;
      }
      return new PsiColorReference(element);
    }
  }
}
