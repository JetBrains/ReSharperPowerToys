using System.Drawing;
using System.Windows.Forms;
using JetBrains.CommonControls;
using JetBrains.DataFlow;
using JetBrains.UI;
using JetBrains.UI.CommonControls;
using JetBrains.UI.Controls;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  public class ZenCodingWrapForm : Form, IConstrainableControl
  {
    private readonly LifetimeDefinition myLifetimeDefinition;

    public ZenCodingWrapForm(Lifetime lifetime)
    {
      myLifetimeDefinition = Lifetimes.Define(lifetime, "ZenCodingWrapForm");
      InitControls();
    }

    public TextBox TextBox { get; private set; }

    private void InitControls()
    {
      using (new LayoutSuspender(this))
      {
        // Self form properties
        Dock = DockStyle.Fill;
        TabStop = true;
        FormBorderStyle = FormBorderStyle.None;
        AutoScaleMode = AutoScaleMode.None;
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = false;
        Size = new Size(16, 16); // Large initial sizes affect layouting badly
        KeyPreview = true;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.Opaque | ControlStyles.ContainerControl, true);
        Text = "ReSharper – Enter Zen Coding abbreviation";
        TextBox = new TextBox { Dock = DockStyle.Fill };
        Controls.Add(TextBox);
        DialogResult = DialogResult.Cancel;
        myLifetimeDefinition.Lifetime.AddDispose(new EditboxGlyph(TextBox, ImageLoader.GetImage("zencoding")));

        // Note: we're not setting the owner to the main window because the menu must be over whatever top level window has spawned it; a menu is topmost so this is not a problem.      
      }
      PerformLayout();
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case (char)Keys.Escape:
          DialogResult = DialogResult.Cancel;
          Close();
          break;
        case (char)Keys.Enter:
          DialogResult = DialogResult.OK;
          Close();
          break;
        default:
          base.OnKeyPress(e);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      myLifetimeDefinition.Terminate();
    }
    #region IConstrainableControl Members

    public Size GetDesiredSize(IGraphicsContainer gc, Size limit)
    {
      return Size.Constrain(limit);
    }

    public void SetLayout(LayoutResult layout)
    {
      // NOP
    }

    public bool AutoActivate
    {
      get; set;
    }
    /// <summary>
    /// Fires when the control would like to be reasked of its desired size.
    /// </summary>
    private readonly ISimpleSignal myWantsResize = new SimpleSignal("WantsResize");

    /// <summary>
    /// Fires when the control would like to be reasked of its desired size.
    /// </summary>
    public ISimpleSignal WantsResize
    {
      get { return myWantsResize; }
    }

    #endregion
  }
}