using System.Drawing;
using JetBrains.UI.RichText;

namespace JetBrains.ReSharper.Psi.Jam.CodeStyle
{
  public class JamCodeStyleColors
  {
    public static readonly TextStyle Text = TextStyle.Default;
    public static readonly TextStyle ShortcutText = TextStyle.FromForeColor(Color.Gray);

    public static readonly TextStyle String = TextStyle.FromForeColor(Color.Orange);
    public static readonly TextStyle Keyword = TextStyle.FromForeColor(Color.Blue);
    public static readonly TextStyle ProcedureName = TextStyle.FromForeColor(Color.Teal);
    public static readonly TextStyle GlobalVariable = TextStyle.FromForeColor(Color.DarkViolet);
    public static readonly TextStyle LocalVariable = TextStyle.FromForeColor(Color.Maroon);
  }
}