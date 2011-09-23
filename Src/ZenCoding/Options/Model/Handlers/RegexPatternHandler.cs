using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers
{
  internal class RegexPatternHandler : IPatternHandler
  {
    private readonly IDictionary<string, Regex> myCache = new Dictionary<string, Regex>();

    public bool Matches(FileAssociation fileAssociation, string fileName)
    {
      if (!fileAssociation.Enabled)
        return false;

      if (fileAssociation.PatternType != PatternType.Regex)
        return false;

      var expr = GetRegex(fileAssociation);

      return expr.IsMatch(fileName);
    }

    private Regex GetRegex(FileAssociation fileAssociation)
    {
      if (myCache.ContainsKey(fileAssociation.Pattern))
        return myCache[fileAssociation.Pattern];

      return new Regex(fileAssociation.Pattern,
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }
  }
}