using System;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  [Flags]
  public enum FindTextSearchFlags
  {
    None = 0,
    StringLiterals = 1,
    Comments = 2,
    Other = 4,
    All = StringLiterals|Comments|Other
  }
}