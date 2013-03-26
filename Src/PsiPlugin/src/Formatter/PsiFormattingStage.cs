using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.FileTypes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormattingStage
  {
    private readonly PsiFormattingVisitor myFmtVisitor;

    private PsiFormattingStage(CodeFormattingContext context)
    {
      myFmtVisitor = CreateFormattingVisitor(context);
    }

    private static PsiFormattingVisitor CreateFormattingVisitor(CodeFormattingContext context)
    {
      IPsiSourceFile sourceFile = context.LastNode.GetSourceFile();
      if (sourceFile != null)
      {
        var projectFileTypeServices = Shell.Instance.GetComponent<IProjectFileTypeServices>();
        var factory = projectFileTypeServices.TryGetService<IPsiCodeFormatterFactory>(sourceFile.LanguageType);
        if (factory != null)
        {
          return factory.CreateFormattingVisitor(context);
        }
      }

      return new PsiFormattingVisitor(context);
    }

    public static void DoFormat(CodeFormattingContext context, IProgressIndicator pi)
    {
      if (context.FirstNode == context.LastNode)
      {
        return;
      }

      var stage = new PsiFormattingStage(context);

      IEnumerable<FormattingRange> nodePairs = context.GetNodePairs();

      IEnumerable<FormatResult<IEnumerable<string>>> spaces = nodePairs.Select(
        range => new FormatResult<IEnumerable<string>>(range, stage.CalcSpaces(new FormattingStageContext(range))));

      FormatterImplHelper.ForeachResult(spaces, pi, res => stage.MakeFormat(res.Range, res.ResultValue));
    }

    private IEnumerable<string> CalcSpaces(FormattingStageContext context)
    {
      var psiTreeNode = context.Parent as IPsiTreeNode;
      if(context.RightChild is IQuantifier)
      {
        return new  List<string> {""};
      } 
      return psiTreeNode != null ? psiTreeNode.Accept(myFmtVisitor, context) : null;

    }

    private void MakeFormat(FormattingRange range, IEnumerable<string> space)
    {
      PsiFormatterHelper.ReplaceSpaces(range.First, range.Last, space);
    }
  }
}
