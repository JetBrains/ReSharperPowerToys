using System.Diagnostics;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.CreateDeclaration;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class CreatePsiRuleContext : CreateContextBase
  {
    private readonly CreatePsiRuleTarget myTarget;
    private readonly IDocument myDocument;
    private readonly ITreeNode myAnchor;

    public CreatePsiRuleContext(CreatePsiRuleTarget target)
    {
      myTarget = target;
      myAnchor = target.Anchor;
      Debug.Assert(myAnchor != null, "myAnchor != null");
      var psiSourceFile = myAnchor.GetSourceFile();
      Debug.Assert(psiSourceFile != null, "psiSourceFile != null");
      myDocument = psiSourceFile.Document;
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
  }
}