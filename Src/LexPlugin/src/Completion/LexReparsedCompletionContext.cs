using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Completion
{
  public class LexReparsedCompletionContext : ReparsedCodeCompletionContext
  {
    public LexReparsedCompletionContext([NotNull] IFile file, TreeTextRange range, string newText)
      : base(file, range, newText)
    {
    }

    protected override IReparseContext GetReparseContext(IFile file, TreeTextRange range)
    {
      return new TrivialReparseContext(file, range);
    }

    protected override IReference FindReference(TreeTextRange referenceRange, ITreeNode treeNode)
    {
      return treeNode.FindReferencesAt(referenceRange).FirstOrDefault();
    }
  }
}
