using JetBrains.ReSharper.Feature.Services.CodeStructure;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeStructure
{
  public class JamCodeStructureRootElement : CodeStructureRootElement
  {
    public JamCodeStructureRootElement(IFile file) : base(file) {}
  }
}