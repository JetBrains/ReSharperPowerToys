using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace JetBrains.ReSharper.PsiPlugin.Grammar
{
  [ProjectFileTypeDefinition(Name)]
  public class PsiProjectFileType : KnownProjectFileType
  {
    public new const string Name = "PSI";
    public new static readonly PsiProjectFileType Instance;

    private PsiProjectFileType() : base(Name, "Psi", new[] {PSI_EXTENSION})
    {
    }

    protected PsiProjectFileType(string name) : base(name)
    {
    }

    protected PsiProjectFileType(string name, string presentableName) : base(name, presentableName)
    {
    }

    protected PsiProjectFileType(string name, string presentableName, IEnumerable<string> extensions) : base(name, presentableName, extensions)
    {
    }

    public const string PSI_EXTENSION = ".psi";
  }
}
