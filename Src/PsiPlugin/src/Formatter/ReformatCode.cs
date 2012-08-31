using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCleanup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  [CodeCleanupModule]
  public class ReformatCode : ICodeCleanupModule
  {
    private static readonly Descriptor OurDescriptor = new Descriptor();

    #region ICodeCleanupModule Members

    public void SetDefaultSetting(CodeCleanupProfile profile, CodeCleanup.DefaultProfileType profileType)
    {
      switch (profileType)
      {
        case CodeCleanup.DefaultProfileType.FULL:
        case CodeCleanup.DefaultProfileType.REFORMAT:
          profile.SetSetting(OurDescriptor, true);
          break;
        default:
          throw new ArgumentOutOfRangeException("profileType");
      }
    }

    public bool IsAvailable(IPsiSourceFile sourceFile)
    {
      return sourceFile.IsLanguageSupported<PsiLanguage>();
    }

    public void Process(IPsiSourceFile sourceFile, IRangeMarker rangeMarkerMarker, CodeCleanupProfile profile, IProgressIndicator progressIndicator)
    {
      ISolution solution = sourceFile.GetSolution();
      if (!profile.GetSetting(OurDescriptor))
      {
        return;
      }

      IPsiFile[] files = sourceFile.GetPsiFiles<PsiLanguage>().Cast<IPsiFile>().ToArray();
      using (progressIndicator.SafeTotal("Reformat Psi", files.Length))
      {
        foreach (IPsiFile file in files)
        {
          using (IProgressIndicator pi = progressIndicator.CreateSubProgress(1))
          {
            using (WriteLockCookie.Create())
            {
              var languageService = file.Language.LanguageService();
              Assertion.Assert(languageService != null, "languageService != null");
              var formatter = languageService.CodeFormatter;
              Assertion.Assert(formatter != null, "formatter != null");

              PsiManager.GetInstance(sourceFile.GetSolution()).DoTransaction(
                delegate
                {
                  if (rangeMarkerMarker != null && rangeMarkerMarker.IsValid)
                  {
                    formatter.Format(
                      solution,
                      rangeMarkerMarker.DocumentRange,
                      CodeFormatProfile.DEFAULT,
                      true,
                      false,
                      pi);
                  }
                  else
                  {
                    formatter.FormatFile(
                      file,
                      CodeFormatProfile.DEFAULT,
                      pi);
                  }
                }, "Code cleanup");
            }
          }
        }
      }
    }

    public PsiLanguageType LanguageType
    {
      get { return PsiLanguage.Instance; }
    }

    public ICollection<CodeCleanupOptionDescriptor> Descriptors
    {
      get { return new CodeCleanupOptionDescriptor[] { OurDescriptor }; }
    }

    public bool IsAvailableOnSelection
    {
      get { return true; }
    }

    #endregion

    #region Nested type: Descriptor

    [DisplayName("Reformat code")]
    [DefaultValue(false)]
    [Category("Psi")]
    private class Descriptor : CodeCleanupBoolOptionDescriptor
    {
      public Descriptor()
        : base("PsiReformatCode")
      {
      }
    }

    #endregion
  }
}
