using System;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers
{
  internal class FileExtensionPatternHandler : IPatternHandler
  {
    public bool Matches(FileAssociation fileAssociation, string fileName)
    {
      if (!fileAssociation.Enabled)
        return false;

      if (fileAssociation.PatternType != PatternType.FileExtension)
        return false;

      return new FileSystemPath(fileName).ExtensionWithDot.Equals(fileAssociation.Pattern, StringComparison.OrdinalIgnoreCase);
    }
  }
}