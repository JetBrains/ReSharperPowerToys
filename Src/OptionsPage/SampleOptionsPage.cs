using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  /// <summary>
  /// This is a sample R# Options Page. 
  /// See the CyclomaticComplexity PowerToy for a live piece of code in a more complex scenario.
  /// </summary>
  [OptionsPage(ID, "Sample Page", "JetBrains.ReSharper.PowerToys.OptionsPage.samplePage.gif", Sequence = 2)]
  public class SampleOptionsPage : UserControl, IOptionsPage
  {
    private CheckBox checkBox1;

    public const string ID = "SamplePageId";
    private Label label1;

    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private Container components;

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

    public EitherControl Control
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
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // checkBox1
      // 
      this.checkBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.checkBox1.Location = new System.Drawing.Point(0, 0);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(600, 24);
      this.checkBox1.TabIndex = 0;
      this.checkBox1.Text = "Sample option";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label1.Location = new System.Drawing.Point(243, 196);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(115, 91);
      this.label1.TabIndex = 1;
      this.label1.Text = ":-)";
      // 
      // SampleOptionsPage
      // 
      this.Controls.Add(this.label1);
      this.Controls.Add(this.checkBox1);
      this.Name = "SampleOptionsPage";
      this.Size = new System.Drawing.Size(600, 472);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
  }
}