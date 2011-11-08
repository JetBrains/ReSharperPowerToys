using System;
using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.DocumentManagers;
using JetBrains.Interop.WinApi;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.TextControl;
using JetBrains.Util;
using System.Linq;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  public abstract class ZenCodingActionBase : IActionHandler
  {
    private static readonly IDictionary<ProjectFileType, DocType> ourFileTypes =
      new Dictionary<ProjectFileType, DocType>
      {
        { HtmlProjectFileType.Instance, DocType.Html },
        { CssProjectFileType.Instance, DocType.Css },
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
      if (!context.CheckAllNotNull(ProjectModel.DataContext.DataConstants.SOLUTION, TextControl.DataContext.DataConstants.TEXT_CONTROL))
        return false;

      return IsSupportedFile(GetProjectFile(context));
    }

    private static bool IsSupportedFile(IProjectFile file)
    {
      return ourFileTypes.Any(_ => file.LanguageType.IsProjectFileType(_.Key)) || Options.Model.Settings.Instance.IsSupportedFile(file.Name);
    }

    protected static DocType GetDocTypeForFile(IProjectFile file)
    {
      if (!IsSupportedFile(file))
      {
        throw new NotSupportedException(String.Format("The file {0} is not supported", file.Name));
      }

      var docType = ourFileTypes
        .Where(_ => file.LanguageType.IsProjectFileType(_.Key))
        .Select(_ => _.Value)
        .FirstOrDefault();

      return docType == DocType.None ? Options.Model.Settings.Instance.GetDocType(file.Name) : docType;
    }

    protected static IProjectFile GetProjectFile(IDataContext context)
    {
      var solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
        return null;

      var dm = solution.GetComponent<DocumentManager>();
      var doc = context.GetData(IDE.DataConstants.DOCUMENT);
      if (doc == null)
        return null;

      return dm.GetProjectFile(doc);
    }

    public abstract void Execute(IDataContext context, DelegateExecute nextExecute);

    protected static void CheckAndIndent(ISolution solution, IProjectFile projectFile, ITextControl textControl, TextRange abbrRange, string expanded, int insertPoint)
    {
      if (expanded.IsEmpty())
      {
        Win32Declarations.MessageBeep(MessageBeepType.Error);
        return;
      }
      
      var indentSize = GlobalFormatSettingsHelper.GetService(solution)
        .GetSettingsForLanguage(PsiProjectFileTypeCoordinator.Instance.GetPrimaryPsiLanguageType(projectFile)).IndentSize;
      expanded = GetEngine(solution).PadString(expanded, (int)textControl.Document.GetCoordsByOffset(abbrRange.StartOffset).Column / indentSize);
      textControl.Document.ReplaceText(abbrRange, expanded);
      if (insertPoint != -1)
      {
        textControl.Caret.MoveTo(abbrRange.StartOffset + insertPoint, CaretVisualPlacement.Generic);
      }
    }
  }
}
