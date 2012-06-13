using System.Drawing;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Completion.LookupItems
{
  internal sealed class PsiKeywordLookupItem : TextLookupItemBase, IKeywordLookupItem
  {
    public PsiKeywordLookupItem(string text, string suffix)
    {
      InsertText = suffix;
      ReplaceText = suffix;
      InsertCaretOffset = suffix.Length;
      ReplaceCaretOffset = suffix.Length;
      Text = text;
    }

    public override IconId Image
    {
      get { return null; }
    }

    protected override RichText GetDisplayName()
    {
      RichText ret = base.GetDisplayName();
      LookupUtil.AddEmphasize(ret, new TextRange(0, ret.Length));
      return ret;
    }
  }
}
