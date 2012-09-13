using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Intentions.CreateFromUsage;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi.Highlightings;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.Util;
using JetBrains.Util.Lazy;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  [QuickFix]
  internal class CreatePsiRuleFromUsage : CreateFromUsageActionBase<CreatePsiRuleContext, ICreatePsiRuleIntention, PsiRuleReference>, IQuickFix
  {
    public CreatePsiRuleFromUsage(PsiUnresolvedRuleReferenceHighlighting error): base(error.Reference)
    {
    }

    protected override CreatePsiRuleContext GetContext()
    {
      return new CreatePsiRuleContext(GetTarget() as CreatePsiRuleTarget);
    }

    protected override ICreationTarget GetTarget()
    {
      return new CreatePsiRuleTarget(Reference);
    }

    protected override IEnumerable<IBulbAction> CreateBulbItems()
    {
      Debug.Assert(Reference != null, "Reference != null");
      yield return new CreatePsiRuleItem(Lazy.Of(GetContext), string.Format("Create rule {0}", Reference.GetName()));
    }

    public void CreateBulbItems(BulbMenu menu, Severity severity)
    {
      menu.ArrangeQuickFixes(Items.Select(_ => Pair.Of(_, severity)));
    }

    public bool IsAvailable(IUserDataHolder cache)
    {
      return ((Reference != null) && (Reference.IsValid()));
    }
  }
}
