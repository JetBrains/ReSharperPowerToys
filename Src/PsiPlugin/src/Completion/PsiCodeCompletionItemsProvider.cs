using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  [Language(typeof (PsiLanguage))]
  internal class PsiCodeCompletionItemsProvider : ItemsProviderOfSpecificContext<PsiCodeCompletionContext>
  {
    protected override bool IsAvailable(PsiCodeCompletionContext context)
    {
      if (!((context.BasicContext.CodeCompletionType == CodeCompletionType.BasicCompletion) || (context.BasicContext.CodeCompletionType == CodeCompletionType.AutomaticCompletion)))
        return false;

      var reference = context.ReparsedContext.Reference;
      if (reference == null)
        return false;

      return true;
    }

    protected override bool AddLookupItems(PsiCodeCompletionContext context, GroupedItemsCollector collector)
    {
      IReference reference = context.ReparsedContext.Reference;
      if (reference == null)
        return false;

      reference.GetReferenceSymbolTable(false).ForAllSymbolInfos(info =>
      {
        var item = new DeclaredElementLookupItemImpl(new DeclaredElementInstance(info.GetDeclaredElement(), EmptySubstitution.INSTANCE), context, PsiLanguage.Instance, context.BasicContext.LookupItemsOwner);
        item.InitializeRanges(context.Ranges, context.BasicContext);
        collector.AddAtDefaultPlace(item);
        
      });
      return true;
    }
  }
}
