using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  class PsiColorReference : IColorReference
  {
    private ITreeNode myElement;

    public PsiColorReference()
    {
      
    }

    public PsiColorReference(ITreeNode element)
    {
      myElement = element;
    }

    public PsiColorReference(HighlightingInfo highlightingInfo)
    {
      
    }
    public void Bind(IColorElement colorElement)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IColorElement> GetColorTable()
    {
      throw new NotImplementedException();
    }

    public ITreeNode Owner
    {
      get { throw new NotImplementedException(); }
    }

    public DocumentRange? ColorConstantRange
    {
      get { return myElement.GetNavigationRange(); }
    }

    public IColorElement ColorElement
    {
      get { throw new NotImplementedException(); }
    }

    public ColorBindOptions BindOptions
    {
      get { throw new NotImplementedException(); }
    }
  }
}
