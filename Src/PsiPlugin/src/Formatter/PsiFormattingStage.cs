using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  public class PsiFormattingStage
  {
    private readonly CodeFormattingContext myContext;
    private readonly PsiFormattingVisitor myFmtVisitor;

    private PsiFormattingStage(FormattingStageData data)
    {
      myContext = data.Context;
      myFmtVisitor = CreateFormattingVisitor(data);
    }

    private CodeFormattingContext Context
    {
      get { return myContext; }
    }

    private static PsiFormattingVisitor CreateFormattingVisitor(FormattingStageData data)
    {
      var sourceFile = data.Context.FirstNode.GetSourceFile();
      if (sourceFile != null)
      {
        var projectFileTypeServices = Shell.Instance.GetComponent<IProjectFileTypeServices>();
        var factory = projectFileTypeServices.TryGetService<IPsiCodeFormatterFactory>(sourceFile.LanguageType);
        if (factory != null)
          return factory.CreateFormattingVisitor(data);
      }

      return new PsiFormattingVisitor(data);
    }

    public static void DoFormat(FormattingStageData data, IProgressIndicator pi)
    {
      if (data.Context.FirstNode == data.Context.LastNode)
        return;

      var stage = new PsiFormattingStage(data);

      var nodePairs = data.Context.HierarchicalEnumNodes().Where(p => data.Context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();

      var spaces = nodePairs.Select(
        range => new FormatResult<IEnumerable<string>>(range, stage.CalcSpaces(new PsiFmtStageContext(range))));

      FormatterImplHelper.ForeachResult(spaces, pi, res => stage.MakeFormat(res.Range, res.ResultValue));
    }

    private IEnumerable<string> CalcSpaces(PsiFmtStageContext context)
    {
      var psiTreeNode = context.Parent as IPsiTreeNode;
      return psiTreeNode != null ? psiTreeNode.Accept(myFmtVisitor, context) : null;
    }

    private void MakeFormat(FormattingRange range, IEnumerable<string> space)
    {
      PsiFormatterHelper.ReplaceSpaces(range.First, range.Last, space);

      // TODO: Move antiglueing logic into CalcSpaces()
      ITokenNode nextToken;
      var prevToken = range.First.FindLastTokenIn();
      if (prevToken != null)
        nextToken = prevToken.GetNextToken();
      else
      {
        nextToken = range.First.NextSibling.FindFirstTokenIn();
        if (nextToken != null)
          prevToken = nextToken.GetPrevToken();
      }

      if (prevToken != null && nextToken != null)
      {
        var separator = Context.CodeFormatter.GetMinimalSeparator(prevToken, nextToken);
        if (separator != null)
          LowLevelModificationUtil.AddChildAfter(range.First, separator);
      }
    }
  }
}
