using System;
using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.DocumentManagers;
using JetBrains.Interop.WinApi;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  public abstract class ZenCodingActionBase : IActionHandler
  {
    private static readonly IDictionary<ProjectFileType, DocType> ourFileTypes =
      new Dictionary<ProjectFileType, DocType>
      {
        { AspProjectFileType.Instance, DocType.Html },
        { HtmlProjectFileType.Instance, DocType.Html },
        { XmlProjectFileType.Instance, DocType.Xsl },
      };

    private static readonly Key<ZenCodingEngine> ourKey = new Key<ZenCodingEngine>("ZenCodingEngine");

    protected static ZenCodingEngine GetEngine(ISolution solution)
    {
      return solution.GetOrCreateData(ourKey, () => Shell.Instance.GetComponent<ZenCodingEngine>());
    }

    public virtual bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // Check that we have a solution
      if (!context.CheckAllNotNull(IDE.DataConstants.SOLUTION, IDE.DataConstants.TEXT_CONTROL))
      {
        return nextUpdate();
      }

      return IsSupportedFile(GetProjectFile(context)) || nextUpdate();
    }

    private static bool IsSupportedFile(IProjectFile file)
    {
      return ourFileTypes.ContainsKey(file.LanguageType) || Settings.Instance.IsSupportedFile(file.Name);
    }

    protected static DocType GetDocTypeForFile(IProjectFile file)
    {
      if (!IsSupportedFile(file))
      {
        throw new NotSupportedException(String.Format("The file {0} is not supported", file.Name));
      }

      if (ourFileTypes.ContainsKey(file.LanguageType))
      {
        return ourFileTypes[file.LanguageType];
      }
      
      return Settings.Instance.GetDocType(file.Name);
    }

    protected static IProjectFile GetProjectFile(IDataContext context)
    {
      ISolution solution = context.GetData(IDE.DataConstants.SOLUTION);
      var dm = solution.GetComponent<DocumentManager>();
      var doc = context.GetData(IDE.DataConstants.DOCUMENT);
      return dm.GetProjectFile(doc);
    }

    public abstract void Execute(IDataContext context, DelegateExecute nextExecute);

    protected static void CheckAndIndent(ISolution solution, ITextControl textControl, TextRange abbrRange, string expanded, int insertPoint)
    {
      if (expanded.IsEmpty())
      {
        Win32Declarations.MessageBeep(MessageBeepType.Error);
        return;
      }
      
      var indentSize = GlobalFormatSettingsHelper.GetService(solution).GetSettingsForLanguage(KnownLanguage.ANY).IndentSize;
      expanded = GetEngine(solution).PadString(expanded, (int)textControl.Document.GetCoordsByOffset(abbrRange.StartOffset).Column / indentSize);
      textControl.Document.ReplaceText(abbrRange, expanded);
      if (insertPoint != -1)
      {
        textControl.Caret.MoveTo(abbrRange.StartOffset + insertPoint, CaretVisualPlacement.Generic);
      }
    }
  }
}
