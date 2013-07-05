using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.Bulbs
{
  public class PsiContextActionDataProvider : CachedContextActionDataProviderBase, IContextActionDataProvider<IPsiFile>
  {
    public PsiContextActionDataProvider([NotNull] ISolution solution, [NotNull] ITextControl textControl, [NotNull] IFile psiFile) : base(solution, textControl, psiFile)
    {
    }

    #region Implementation of IContextActionDataProvider<out IPsiFile>

    public IPsiFile PsiFile { get { return (IPsiFile)base.PsiFile; } }

    #endregion
  }
}
