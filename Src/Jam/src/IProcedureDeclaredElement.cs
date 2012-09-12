using System.Collections.Generic;

namespace JetBrains.ReSharper.Psi.Jam
{
  public interface IProcedureDeclaredElement : IJamDeclaredElement 
  {
    IList<IParameterDeclaredElement> Parameters { get; }
  }
}