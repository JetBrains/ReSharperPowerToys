using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  public interface IPsiPsiFileProperties : ICustomPsiSourceFileProperties
  {
    bool AllowUnsafeCode { get; }
    bool TreatWarningsAsErrors { get; }
    string SuppressWarnings { get; }
    int WarningLevel { get; }
    bool XmlDocGenerationEnabled { get; }
  }

  public static class PsiPsiFilePropertiesExtension
  {
    public static IEnumerable<string> ParseCompilerIdList(string s)
    {
      foreach (string str in s.Split(',', ';', ' ', '\t'))
      {
        string warning = str.Trim();
        if (String.IsNullOrEmpty(warning))
          continue;

        int number;
        if (Int32.TryParse(warning, out number))
          yield return "PSI" + number.ToString("0000");
        else
          yield return warning;
      }
    }
  }
}
