namespace JetBrains.ReSharper.Psi.Jam.Tree
{
  public partial interface IParameter : IJamDeclaration
  {
    new IParameterDeclaredElement DeclaredElement { get; }
  }
}