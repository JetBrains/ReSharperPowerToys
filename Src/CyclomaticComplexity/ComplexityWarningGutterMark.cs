using System;
using System.Drawing;
using System.Reflection;

using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.TextControl.Markup;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  public class ComplexityWarningGutterMark : IconGutterMark
  {
    private static Image ourImage;

    static ComplexityWarningGutterMark()
    {
      ourImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (ComplexityWarning), "ComplexityWarning.png"));
    }

    public ComplexityWarningGutterMark()
      : base(ourImage)
    {
    }

    public override void OnClick(IHighlighter highlighter)
    {
    }

    public override bool IsClickable
    {
      get { return false; }
    }
  }
}