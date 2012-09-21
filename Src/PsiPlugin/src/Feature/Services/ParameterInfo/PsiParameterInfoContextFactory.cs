using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ParameterInfo;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Completion;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Feature.Services.ParameterInfo
{
  [ParameterInfoContextFactory(typeof(PsiLanguage))]
  public class PsiParameterInfoContextFactory : IParameterInfoContextFactory
  {
    private static readonly char[] OurImportantChars = new[] { '[', ']' };
    private static readonly char[] OurChars = new[] { '[', ',' };

    #region Implementation of IParameterInfoContextFactory

    public IParameterInfoContext CreateContext(ISolution solution, IDocument document, int caretOffset, int expectedLParenthOffset, char invocationChar, IContextBoundSettingsStore contextBoundSettingsStore)
    {
      if (!solution.GetComponent<PsiIntellisenseManager>().GetIntellisenseEnabled(contextBoundSettingsStore))
        return null;

      var documentRange = new DocumentRange(document, caretOffset);
      var file = solution.GetPsiServices().PsiManager.GetPsiFile<PsiLanguage>(documentRange);
      if (file == null)
        return null;

      var contextRange = file.Translate(documentRange);
      if (!contextRange.IsValid())
        return null;

      var tokenElement = file.FindTokenAt(contextRange.StartOffset) as ITokenNode;

      if (tokenElement == null)
        return null;

      var ruleNameUsage = tokenElement.GetContainingNode<RuleNameUsage>();
      if(ruleNameUsage == null)
      {
        return null;
      }

      return new PsiParameterInfoContext(ruleNameUsage, 0);
    }

    public bool ShouldPopup(IDocument document, int caretOffset, char c, ISolution solution, IContextBoundSettingsStore contextBoundSettingsStore)
    {
      if (!OurChars.Contains(c))
        return false;
      if (!solution.GetComponent<PsiIntellisenseManager>().GetIntellisenseEnabled(contextBoundSettingsStore))
        return false;

      return true;
    }

    public PsiLanguageType Language
    {
      get { return PsiLanguage.Instance; }
    }

    public IEnumerable<char> ImportantChars
    {
      get
      {
        return OurImportantChars;
      }
    }

    #endregion
  }
}
