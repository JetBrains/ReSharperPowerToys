using System;
using System.Drawing;
using System.Reflection;

using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.TextControl.Markup;

namespace JetBrains.ReSharper.PowerToys.CyclomaticComplexity
{
  public class ComplexityErrorGutterMark : IconGutterMark
  {
    private static Image ourImage;

    static ComplexityErrorGutterMark()
    {
      ourImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (ComplexityWarning), "ComplexityError.png"));
    }

    public ComplexityErrorGutterMark()
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