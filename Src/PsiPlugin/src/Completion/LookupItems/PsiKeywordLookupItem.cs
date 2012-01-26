using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Completion.LookupItems
{
  class PsiKeywordLookupItem: TextLookupItemBase, IKeywordLookupItem
  {
    public PsiKeywordLookupItem(string text, string suffix)
    {
      InsertText = suffix;
      ReplaceText = suffix;
      InsertCaretOffset = suffix.Length;
      ReplaceCaretOffset = suffix.Length;
      Text = text;
    }

    protected override RichText GetDisplayName()
    {
      var ret = base.GetDisplayName();
      LookupUtil.AddEmphasize(ret, new TextRange(0, ret.Length));
      return ret;
    }

    public override Image Image
    {
      get { return null; }
    }
  }
}
