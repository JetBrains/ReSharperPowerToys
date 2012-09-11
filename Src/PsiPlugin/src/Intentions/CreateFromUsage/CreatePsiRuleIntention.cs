using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  [Language(typeof(PsiLanguage))]
  internal class CreatePsiRuleIntention : ICreatePsiRuleIntention
  {
    public PsiIntentionResult ExecuteEx(CreatePsiRuleContext context)
    {
      return PsiRuleBuilder.Create(context);
    }
  }
}