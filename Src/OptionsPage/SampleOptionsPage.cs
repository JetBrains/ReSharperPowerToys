using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.UI.Options;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  /// <summary>
  /// This is a sample R# Options Page. 
  /// See the CyclomaticComplexity PowerToy for a live piece of code in a more complex scenario.
  /// </summary>
  [OptionsPage(
    ID,
    "Sample Page",
    "JetBrains.ReSharper.PowerToys.OptionsPage.samplePage.gif",
    Sequence = 2)]
  public class SampleOptionsPage : UserControl, IOptionsPage
  {
    private CheckBox checkBox1;

    public const string ID = "SamplePageId";

    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private Container components = null;

    public SampleOptionsPage(IOptionsDialog optionsUI)
    {
      // You can use IOptionsUI instance passed into the constructor for the following purposes:
      //   - access current solution (if any)
      //   - access instance of any other IOptionsPage by its ID. This can be helpful 
      //     if your page's appearance and/or behavior depends on values set on another page.

      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // load values to controls
      SampleSettings settings = SampleSettings.Instance;
      checkBox1.Checked = settings.SampleOption;
    }

    public Control Control
    {
      get { return this; }
    }

    public string Id
    {
      get { return ID; }
    }

    public bool OnOk()
    {
      // save values from controls
      SampleSettings settings = SampleSettings.Instance;
      settings.SampleOption = checkBox1.Checked;
      return true;
    }

    public void OnActivated(bool activated)
    {
    }

    public void InitializeUI()
    {
    }

    public bool ValidatePage()
    {
      return true;
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
          components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      checkBox1 = new CheckBox();
      SuspendLayout();
      // 
      // checkBox1
      // 
      checkBox1.Dock = DockStyle.Top;
      checkBox1.Location = new Point(0, 0);
      checkBox1.Name = "checkBox1";
      checkBox1.Size = new Size(600, 24);
      checkBox1.TabIndex = 0;
      checkBox1.Text = "Sample option";
      // 
      // SampleOptionsPage
      // 
      Controls.Add(checkBox1);
      Name = "SampleOptionsPage";
      Size = new Size(600, 472);
      ResumeLayout(false);
    }

    #endregion
  }
}