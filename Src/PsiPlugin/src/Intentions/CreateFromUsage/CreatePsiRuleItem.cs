using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Intentions.CreateFromUsage;
using JetBrains.ReSharper.Intentions.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class CreatePsiRuleItem : CreateFromUsageItemBase<CreatePsiRuleContext>, IPartBulbItem
  {
    private readonly string myFormatText;

    public CreatePsiRuleItem(JetBrains.Util.Lazy.Lazy<CreatePsiRuleContext> context, string format) : base(context)
    {
      myFormatText = format;
    }

    public override string Text
    {
      get
      {
        return myFormatText;
      }
    }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      var result = ExecutePsiIntention();

      Assertion.Assert(result.ResultDeclaration != null, "result.ResultDeclaration != null");
      Assertion.Assert(result.ResultDeclaration.IsValid(), "result.ResultDeclaration.IsValid()");
      result.ResultDeclaration.GetPsiServices().Caches.Update();
      var postExecute = GetContext().Target as ITargetPostExecute;
      if (result.ResultDeclaration.DeclaredElement != null && postExecute != null)
      {
        postExecute.PostExecute(result.ResultDeclaration.DeclaredElement);
      }

      return tmp => result.ExecuteTemplate();
    }

    private PsiIntentionResult ExecutePsiIntention()
    {
      return LanguageManager.Instance.GetService<ICreatePsiRuleIntention>(
        GetContext().Target.GetTargetDeclaration().Language).ExecuteEx(GetContext());
    }

    protected override IntentionResult ExecuteIntention()
    {
      return null;
    }
  }
}