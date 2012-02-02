using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  public class PsiFileIndexProcess : PsiDaemonStageProcessBase
  {
    //public OneToListMap<IFunctionExpression, IReturnStatement> FunctionReturns { private set; get; }
    //public List<IFunctionExpression> Functions { private set; get; }
    //public OneToSetMap<IFunctionExpression, string> FieldsDefinedInFunction { private set; get; }

    //private readonly Stack<IFunctionExpression> myFunctions = new Stack<IFunctionExpression>();

    public PsiFileIndexProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process, settingsStore)
    {
      //Functions = new List<IFunctionExpression>();
      //FunctionReturns = new OneToListMap<IFunctionExpression, IReturnStatement>();
      //FieldsDefinedInFunction = new OneToSetMap<IFunctionExpression, string>();
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
    }

    /*public override void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
    {
      var functionExpression = element as IFunctionExpression;
      if (functionExpression != null)
      {
        Functions.Add(functionExpression);
        myFunctions.Push(functionExpression);
      }
      base.ProcessBeforeInterior(element, consumer);
    }*/

    /*public override void VisitReturnStatement(IReturnStatement returnStatementParam, IHighlightingConsumer context)
    {
      if (!myFunctions.IsEmpty())
      {
        FunctionReturns.Add(myFunctions.Peek(), returnStatementParam);
      }
      base.VisitReturnStatement(returnStatementParam, context);
    }*/

    /*public override void VisitBinaryExpression(IBinaryExpression binaryExpressionParam, IHighlightingConsumer context)
    {
      if (!myFunctions.IsEmpty())
      {
        if (binaryExpressionParam.IsAssignment)
        {
          var referenceExpression = binaryExpressionParam.LeftOperand as IReferenceExpression;
          if (referenceExpression != null)
          {
            // this.xxx = ...
            var name = referenceExpression.Text;
            if (referenceExpression.Qualifier is IThisExpression && name != SharedImplUtil.MISSING_DECLARATION_NAME)
            {
              FieldsDefinedInFunction.Add(myFunctions.Peek(), name);
            }
          }
        }
      }
      base.VisitBinaryExpression(binaryExpressionParam, context);
    }*/

    /*public override void VisitFunctionExpression(IFunctionExpression functionExpressionParam, IHighlightingConsumer context)
    {
      myFunctions.Pop();
    }*/
  }
}
