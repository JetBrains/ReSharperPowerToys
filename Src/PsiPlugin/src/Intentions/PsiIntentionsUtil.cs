using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Intentions
{
  static class PsiIntentionsUtil
  {
    public static IRuleDeclaration AddToTarget(IRuleDeclaration declarationToAdd, ICreationTarget target)
    {

      var inserter = new PsiRuleInserter(declarationToAdd, target);
      return inserter.InsertRule();
    }
  }
}
