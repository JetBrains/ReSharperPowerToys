using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.LiveTemplates;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class PsiRuleBuilder
  {
    public static PsiIntentionResult Create(CreatePsiRuleContext context)
    {
      var declaration = context.Declaration;

      declaration = PsiIntentionsUtil.AddToTarget(declaration, context.Target);


      var holders = new List<ITemplateFieldHolder>();
      if(declaration.Parameters != null)
      {
        var child = declaration.Parameters.FirstChild;
        while(child != null)
        {
          if((child is IRuleName) || (child is IVariableDeclaration))
          {
            holders.Add(new FindersTemplateFieldHolder(new TemplateField(child.GetText(), child.GetNavigationRange().TextRange.StartOffset), new ITemplateFieldFinder[] { new PsiTemplateFinder(child) }));
          }
          child = child.NextSibling;
        }
      }

      return new PsiIntentionResult(holders, declaration, context.Anchor, new DocumentRange(context.Document, declaration.GetNavigationRange().TextRange));
    }
  }

  public class PsiTemplateFinder : ITemplateFieldFinder
  {
    private readonly ITreeNode myNode;

    public PsiTemplateFinder(ITreeNode node)
    {
      myNode = node;
    }

    #region Implementation of ITemplateFieldFinder

    public IEnumerable<ITreeNode> Find(IDeclaration declaration)
    {
      yield return myNode;
    }

    #endregion
  }
}