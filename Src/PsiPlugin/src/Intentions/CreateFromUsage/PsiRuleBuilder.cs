using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.LanguageSpecific;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class PsiRuleBuilder
  {
    public static PsiIntentionResult Create(CreatePsiRuleContext context)
    {
      var declaration = context.Declaration;

      declaration = PsiIntentionsUtil.AddToTarget(declaration, context.Target);


      var holders = new List<ITemplateFieldHolder>();

      return new PsiIntentionResult(holders, declaration, context.Anchor, new DocumentRange(context.Document, declaration.GetNavigationRange().TextRange));
    }
  }
}