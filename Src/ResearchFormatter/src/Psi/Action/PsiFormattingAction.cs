using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ActionManagement;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.ResearchFormatter.Psi.Action
{
  [ContextAction(Group = "PSI", Name = "format", Description = "format psi", Priority = -1)]
  public class PsiFormattingAction : ContextActionBase
  {
    #region Overrides of BulbActionBase

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      throw new NotImplementedException();
    }

    public override string Text
    {
      get { return "format psi"; }
    }

    #endregion

    #region Overrides of ContextActionBase

    public override bool IsAvailable(IUserDataHolder cache)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
