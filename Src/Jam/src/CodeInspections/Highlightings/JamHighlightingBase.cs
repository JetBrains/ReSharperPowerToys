using System;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings
{
  public abstract class JamHighlightingBase : IHighlighting
  {
    string IHighlighting.ToolTip
    {
      get { throw new InvalidOperationException(); }
    }

    string IHighlighting.ErrorStripeToolTip
    {
      get { throw new InvalidOperationException(); }
    }

    int IHighlighting.NavigationOffsetPatch
    {
      get { throw new InvalidOperationException(); }
    }

    public abstract bool IsValid();
  }
}