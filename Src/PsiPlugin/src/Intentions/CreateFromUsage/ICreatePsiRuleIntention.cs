using JetBrains.ReSharper.Feature.Services.Intentions;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public interface ICreatePsiRuleIntention
  {
    PsiIntentionResult ExecuteEx(CreatePsiRuleContext context);
  }
}