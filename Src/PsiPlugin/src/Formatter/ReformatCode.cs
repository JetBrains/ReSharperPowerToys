using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCleanup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  [CodeCleanupModule]
  public class ReformatCode : ICodeCleanupModule
  {
    private static readonly Descriptor OurDescriptor = new Descriptor();

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
      var solution = sourceFile.GetSolution();
      if (!profile.GetSetting(OurDescriptor)) return;

      var files = sourceFile.GetPsiFiles<PsiLanguage>().Cast<IPsiFile>().ToArray();
      using (progressIndicator.SafeTotal("Reformat Psi", files.Length))
      {
        foreach (var file in files)
        {
          using (var indicator = progressIndicator.CreateSubProgress(1))
          using (WriteLockCookie.Create())
          {
            var formatter = file.Language.LanguageService().CodeFormatter;

            PsiManager.GetInstance(sourceFile.GetSolution()).DoTransaction(
              delegate
              {
                if (rangeMarkerMarker != null && rangeMarkerMarker.IsValid)
                  formatter.Format(
                    solution,
                    rangeMarkerMarker.DocumentRange,
                    CodeFormatProfile.DEFAULT,
                    true,
                    false,
                    indicator);
                else
                {
                  formatter.FormatFile(
                    file,
                    CodeFormatProfile.DEFAULT,
                    indicator);
                }
              }, "Code cleanup");
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
  }
}
