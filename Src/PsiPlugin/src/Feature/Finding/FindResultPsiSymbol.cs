using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.PsiPlugin.Cache;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Finding
{
  public class FindResultPsiSymbol : FindResult
  {
    private readonly IPsiSymbol mySymbol;
    private readonly IProjectFile myProjectFile;

    public FindResultPsiSymbol([NotNull] IPsiSymbol symbol, IProjectFile projectFile)
    {
      mySymbol = symbol;
      myProjectFile = projectFile;
    }

    public IProjectFile ProjectFile
    {
      get { return myProjectFile; }
    }

    [NotNull]
    public IPsiSymbol Symbol
    {
      get { return mySymbol; }
    }

    public override bool Equals(object obj)
    {
      return (obj is FindResultPsiSymbol) && ((FindResultPsiSymbol) obj).mySymbol.Equals(mySymbol);
    }

    public override int GetHashCode()
    {
      return mySymbol.GetHashCode();
    }
  }
}
