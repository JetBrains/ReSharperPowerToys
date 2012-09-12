using JetBrains.Annotations;

namespace JetBrains.ReSharper.Psi.Jam
{
  public interface IParameterDeclaredElement : IJamDeclaredElement 
  {
    [CanBeNull]
    IProcedureDeclaredElement ContainingProcedure { get; }
  }
}