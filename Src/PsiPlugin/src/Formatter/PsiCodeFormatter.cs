using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.Formatter
{
  //[Language(typeof (PsiLanguage))]
  public class PsiCodeFormatter : CodeFormatterBase
  {
    private readonly PsiLanguage myLanguage;

    public PsiCodeFormatter(Lifetime lifetime, PsiLanguage language, ISettingsStore settingsStore, ISettingsOptimization settingsOptimization)
      : base(settingsStore)
    {
      myLanguage = language;
    }

    protected override PsiLanguageType LanguageType
    {
      get { return myLanguage; }
    }

    public override bool IsWhitespaceToken(ITokenNode token)
    {
      return token.IsWhitespaceToken();
    }

    protected override bool IsFormatNextSpaces(CodeFormatProfile profile)
    {
      return profile != CodeFormatProfile.INDENT;
    }

    public override ITokenNode GetMinimalSeparator(ITokenNode leftToken, ITokenNode rightToken)
    {
      return PsiFormatterHelper.CreateSpace(" ");
    }

    public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
    {
      return new ITreeNode[] { PsiFormatterHelper.CreateSpace(indent) };
    }

    public override ITreeRange Format(ITreeNode firstElement, ITreeNode lastElement, CodeFormatProfile profile, IProgressIndicator pi, IContextBoundSettingsStore overrideSettingsStore = null)
    {
      ITreeNode firstNode;
      ITreeNode lastNode;

      GetFirstAndLastNode(firstElement, lastElement, out firstNode, out lastNode);

      using (pi.SafeTotal(4))
      {
        var context = new PsiCodeFormattingContext(this, firstNode, lastNode, NullProgressIndicator.Instance);
        if (profile != CodeFormatProfile.INDENT)
        {

          using (IProgressIndicator subPi = pi.CreateSubProgress(2))
          {
            using (subPi.SafeTotal(2))
            {
              PsiFormattingStage.DoFormat(context, subPi.CreateSubProgress(1));
              PsiIndentingStage.DoIndent(context, subPi.CreateSubProgress(1), false);
            }
          }
        }
        else
        {
          using (IProgressIndicator subPi = pi.CreateSubProgress(4))
          {
            PsiIndentingStage.DoIndent(context, subPi, true);
          }
        }
      }
      return new TreeRange(firstElement, lastElement);
    }

    private static void GetFirstAndLastNode(ITreeNode firstElement, ITreeNode lastElement, out ITreeNode firstNode, out ITreeNode lastNode)
    {
      firstNode = firstElement;
      lastNode = lastElement;
      if (firstElement != lastElement)
      {
        if (firstElement == null)
        {
          firstNode = lastElement;
        }
        ITreeNode commonParent = firstNode.FindCommonParent(lastNode);

        firstNode = GetFirstNode(firstNode, commonParent);
        lastNode = GetLastNode(lastNode, commonParent);

      }
      else
      {
        if (firstElement.FirstChild != null)
        {
          firstNode = firstElement.FirstChild;
          lastNode = firstElement.LastChild;
        }
      }
    }

    private static ITreeNode GetLastNode(ITreeNode lastChild, ITreeNode commonParent)
    {
      while ((lastChild.Parent != null) && (lastChild.Parent != commonParent))
      {
        lastChild = lastChild.Parent;
      }

      ITreeNode lastNode = lastChild;
      while (lastNode.LastChild != null)
      {
        lastNode = lastNode.LastChild;
      }
      return lastNode;
    }

    private static ITreeNode GetFirstNode(ITreeNode firstChild, ITreeNode commonParent)
    {
      while ((firstChild.Parent != null) && (firstChild.Parent != commonParent))
      {
        firstChild = firstChild.Parent;
      }

      ITreeNode firstNode = firstChild;
      while (firstNode.FirstChild != null)
      {
        firstNode = firstNode.FirstChild;
      }
      return firstNode;
    }

    public override void FormatInsertedNodes(ITreeNode nodeFirst, ITreeNode nodeLast, bool formatSurround)
    {
      Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
    }

    public override ITreeRange FormatInsertedRange(ITreeNode nodeFirst, ITreeNode nodeLast, ITreeRange origin)
    {
      Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
      return new TreeRange(nodeFirst, nodeLast);
    }

    public override void FormatReplacedNode(ITreeNode oldNode, ITreeNode newNode)
    {
      FormatInsertedNodes(newNode, newNode, false);
    }

    public override void FormatDeletedNodes(ITreeNode parent, ITreeNode prevNode, ITreeNode nextNode)
    {
      Format(
        prevNode,
        nextNode,
        CodeFormatProfile.GENERATOR,
        null);
    }
  }

  public class PsiCodeFormattingContext : CodeFormattingContext
  {
    public PsiCodeFormattingContext(PsiCodeFormatter psiCodeFormatter, ITreeNode firstNode, ITreeNode lastNode, NullProgressIndicator instance)
      : base(psiCodeFormatter, firstNode, lastNode, instance)
    {
    }

    protected override bool CanModifyNode(ITreeNode element, NodeModificationType nodeModificationType)
    {
      return true;
    }
  }
}
