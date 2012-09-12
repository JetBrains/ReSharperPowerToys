namespace JetBrains.ReSharper.Psi.Jam.Tree
{
  public interface ILocalVariableDeclaration : IJamDeclaration
  {
    new ILocalVariableDeclaredElement DeclaredElement { get; }
  }
}