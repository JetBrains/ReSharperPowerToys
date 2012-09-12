using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace JetBrains.ReSharper.Psi.Jam
{
  [ProjectFileTypeDefinition(Name)]
  public class JamProjectFileType : KnownProjectFileType
  {
    public new const string Name = "JAM";
    public const string JamExtension = ".jam";

    public new static readonly JamProjectFileType Instance = new JamProjectFileType();

    private JamProjectFileType() : base(Name, Name, new[] {JamExtension}) {}

    protected JamProjectFileType(string name) : base(name) {}

    protected JamProjectFileType(string name, string presentableName) : base(name, presentableName) {}

    protected JamProjectFileType(string name, string presentableName, IEnumerable<string> extensions) : base(name, presentableName, extensions) {}
  }
}