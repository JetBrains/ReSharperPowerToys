using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  public static class PsiDataContextsEx
  {
    [NotNull]
    public static Func<Lifetime, DataContexts, IDataContext> ToDataContext([NotNull] this ITextControl textControl)
    {
      if (textControl == null)
        throw new ArgumentNullException("textControl");

      return (lifetime, contexts) => contexts.CreateWithDataRules(lifetime, DataRules.AddRule("TextControl", TextControl.DataContext.DataConstants.TEXT_CONTROL, textControl));
    }

    [NotNull]
    public static ContextRange ToContextRange([NotNull] this ITextControl textControl)
    {
      if (textControl == null)
        throw new ArgumentNullException("textControl");

      return ContextRange.Smart(textControl.ToDataContext());
    }
  }
}
