using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ControlFlow2;
using JetBrains.ReSharper.Psi.ControlFlow2.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  public class ComplexityAnalysisElementProcessor : IRecursiveElementProcessor
  {
    #region Data

    /// <summary>
    /// List of highlightings
    /// </summary>
    private readonly List<HighlightingInfo> myHighlightings = new List<HighlightingInfo>();

    private readonly IDaemonProcess myProcess;

    #endregion

    #region Init

    public ComplexityAnalysisElementProcessor(IDaemonProcess process)
    {
      myProcess = process;
    }

    #endregion

    #region Attributes

    public List<HighlightingInfo> Highlightings
    {
      get
      {
        return myHighlightings;
      }
    }

    #endregion

    #region Implementation

    /// <summary>
    /// This method walks the control flow graph counting edges and nodes. Cyclomatic complexity is then calculated from the two values.
    /// </summary>
    private static int CalcCyclomaticComplexity(ICSharpFunctionDeclaration declaration)
    {
      ICSharpControlFlowGraf graf = CSharpControlFlowBuilder.Build(declaration);
      HashSet<IControlFlowRib> ribs = GetRibs(graf);
      int nodes = GetNodesCount(ribs);

      return ribs.Count - nodes + 2;
    }

    private static int GetNodesCount(HashSet<IControlFlowRib> ribs)
    {
      bool hasSrcNull = false;
      bool hasDstNull = false;

      var nodes = new HashSet<ICSharpControlFlowElement>();
      foreach(ICSharpControlFlowRib rib in ribs)
      {
        if(rib.Source != null)
          nodes.Add(rib.Source);
        else
          hasSrcNull = true;

        if(rib.Target != null)
          nodes.Add(rib.Target);
        else
          hasDstNull = true;
      }
      return nodes.Count + (hasDstNull ? 1 : 0) + (hasSrcNull ? 1 : 0);
    }

    private static HashSet<IControlFlowRib> GetRibs(ICSharpControlFlowGraf graf)
    {
      var ribs = new HashSet<IControlFlowRib>();
      foreach(ICSharpControlFlowElement element in graf.AllElements)
      {
        foreach(IControlFlowRib rib in element.Exits)
          ribs.Add(rib);
        foreach(IControlFlowRib rib in element.Entries)
          ribs.Add(rib);
      }
      return ribs;
    }

    private void ProcessFunctionDeclaration(ICSharpFunctionDeclaration declaration)
    {
      // Nothing to calculate
      if(declaration.Body == null)
        return;

      int cyclomatic = CalcCyclomaticComplexity(declaration);

      // Placing highlighting
      if(cyclomatic > ComplexityAnalysisDaemonStage.Threshold)
      {
        string message = string.Format("Member has cyclomatic complexity of {0} ({1}%)", cyclomatic, (int)(cyclomatic * 100.0 / ComplexityAnalysisDaemonStage.Threshold));
        var warning = new ComplexityWarning(message);
        myHighlightings.Add(new HighlightingInfo(declaration.GetNameDocumentRange(), warning));
      }
    }

    #endregion

    #region IRecursiveElementProcessor Members

    public bool InteriorShouldBeProcessed(IElement element)
    {
      return true;
    }

    public void ProcessAfterInterior(IElement element)
    {
      // We are only interested in function declarations (methods, property accessors, etc.)
      var functionDeclaration = element as ICSharpFunctionDeclaration;
      if(functionDeclaration != null)
        ProcessFunctionDeclaration(functionDeclaration);
    }

    public void ProcessBeforeInterior(IElement element)
    {
    }

    public bool ProcessingIsFinished
    {
      get
      {
        return myProcess.InterruptFlag;
      }
    }

    #endregion
  }
}