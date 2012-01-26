﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity("SyntaxError", null, HighlightingGroupIds.LanguageUsage, "Syntax Error", @"
          Syntax error", JetBrains.ReSharper.Daemon.Severity.ERROR, false, Internal = false)]
namespace JetBrains.ReSharper.PsiPlugin.Feature.Services
{
  [ConfigurableSeverityHighlighting("SyntaxError", "PSI", OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = myMessage)]
  class PsiErrorElementHighlighting : IHighlightingWithRange
  {
    private ITreeNode myElement;
    private const string myMessage = "Syntax error";
    private string myError = "Syntax error";

    public PsiErrorElementHighlighting(ITreeNode element)
    {
      myElement = element;
    }

    public PsiErrorElementHighlighting(ITreeNode element, String message)
    {
      myElement = element;
      myError = message;
    }
    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return myError; }
    }

    public string ErrorStripeToolTip
    {
      get { return myError; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public DocumentRange CalculateRange()
    {
      return myElement.GetNavigationRange();
    }
  }
}