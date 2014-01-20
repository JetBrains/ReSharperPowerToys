using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

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
