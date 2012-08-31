using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;

namespace JetBrains.ReSharper.LexPlugin.Grammar
{
  [ProjectFileTypeDefinition(Name)]
  public class LexProjectFileType : KnownProjectFileType
  {
    private new const string Name = "Lex";
    public const string PsiExtension = ".lex";
    public new static readonly LexProjectFileType Instance = new LexProjectFileType();

    private LexProjectFileType()
      : base(Name, "Lex", new[] { PsiExtension })
    {
    }

    protected LexProjectFileType(string name)
      : base(name)
    {
    }

    protected LexProjectFileType(string name, string presentableName)
      : base(name, presentableName)
    {
    }

    protected LexProjectFileType(string name, string presentableName, IEnumerable<string> extensions)
      : base(name, presentableName, extensions)
    {
    }
  }
}
