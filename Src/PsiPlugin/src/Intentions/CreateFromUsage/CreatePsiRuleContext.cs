using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.CreateDeclaration;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class CreatePsiRuleContext : CreateContextBase
  {
    private readonly CreatePsiRuleTarget myTarget;
    private readonly IDocument myDocument;
    private readonly ITreeNode myAnchor;
    private readonly int myOffset;

    public CreatePsiRuleContext(CreatePsiRuleTarget target)
    {
      myTarget = target;
      myAnchor = target.Anchor;
      myOffset = myAnchor.GetNavigationRange().TextRange.EndOffset;
      myDocument = myAnchor.GetSourceFile().Document;
    }

    public ICreationTarget Target
    {
      get { return myTarget; }
    }

    public IRuleDeclaration Declaration
    {
      get { return myTarget.Declaration; }
    }

    public IDocument Document
    {
      get { return myDocument; }
    }

    public ITreeNode Anchor
    {
      get { return myAnchor; }
    }

    public int Offset
    {
      get { return myOffset; }
    }
  }
}