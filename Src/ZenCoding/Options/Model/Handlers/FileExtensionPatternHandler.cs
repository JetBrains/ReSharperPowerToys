using System;
using System.IO;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers
{
  internal class FileExtensionPatternHandler : IPatternHandler
  {
    public bool Matches(FileAssociation fileAssociation, string fileName)
    {
      if (!fileAssociation.Enabled)
      {
        return false;
      }

      if (fileAssociation.PatternType != PatternType.FileExtension)
      {
        return false;
      }

      return Path.GetExtension(fileName).Equals(fileAssociation.Pattern, StringComparison.OrdinalIgnoreCase);
    }
  }
}