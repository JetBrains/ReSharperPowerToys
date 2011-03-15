namespace JetBrains.ReSharper.PowerToys.FindText
{
  partial class EnterSearchStringDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterSearchStringDialog));
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOk = new System.Windows.Forms.Button();
      this.txtSearchString = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.cbCaseSensitive = new System.Windows.Forms.CheckBox();
      this.cbSearchStrings = new System.Windows.Forms.CheckBox();
      this.cbSearchComments = new System.Windows.Forms.CheckBox();
      this.cbSearchOther = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(214, 114);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOk
      // 
      this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOk.Location = new System.Drawing.Point(136, 114);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 2;
      this.btnOk.Text = "Find";
      this.btnOk.UseVisualStyleBackColor = true;
      // 
      // txtSearchString
      // 
      this.txtSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                          | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSearchString.Location = new System.Drawing.Point(6, 20);
      this.txtSearchString.Name = "txtSearchString";
      this.txtSearchString.Size = new System.Drawing.Size(282, 20);
      this.txtSearchString.TabIndex = 1;
      this.txtSearchString.TextChanged += new System.EventHandler(this.FlagsChanged);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                 | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(6, 2);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(281, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enter text to search for:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // cbCaseSensitive
      // 
      this.cbCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cbCaseSensitive.AutoSize = true;
      this.cbCaseSensitive.Location = new System.Drawing.Point(193, 46);
      this.cbCaseSensitive.Name = "cbCaseSensitive";
      this.cbCaseSensitive.Size = new System.Drawing.Size(94, 17);
      this.cbCaseSensitive.TabIndex = 4;
      this.cbCaseSensitive.Text = "Case sensitive";
      this.cbCaseSensitive.UseVisualStyleBackColor = true;
      // 
      // cbSearchStrings
      // 
      this.cbSearchStrings.AutoSize = true;
      this.cbSearchStrings.Checked = true;
      this.cbSearchStrings.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbSearchStrings.Location = new System.Drawing.Point(6, 47);
      this.cbSearchStrings.Name = "cbSearchStrings";
      this.cbSearchStrings.Size = new System.Drawing.Size(137, 17);
      this.cbSearchStrings.TabIndex = 4;
      this.cbSearchStrings.Text = "Search in &String Literals";
      this.cbSearchStrings.UseVisualStyleBackColor = true;
      this.cbSearchStrings.CheckedChanged += new System.EventHandler(this.FlagsChanged);
      // 
      // cbSearchComments
      // 
      this.cbSearchComments.AutoSize = true;
      this.cbSearchComments.Checked = true;
      this.cbSearchComments.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbSearchComments.Location = new System.Drawing.Point(6, 70);
      this.cbSearchComments.Name = "cbSearchComments";
      this.cbSearchComments.Size = new System.Drawing.Size(123, 17);
      this.cbSearchComments.TabIndex = 4;
      this.cbSearchComments.Text = "Search in &Comments";
      this.cbSearchComments.UseVisualStyleBackColor = true;
      this.cbSearchComments.CheckedChanged += new System.EventHandler(this.FlagsChanged);
      // 
      // cbSearchOther
      // 
      this.cbSearchOther.AutoSize = true;
      this.cbSearchOther.Checked = true;
      this.cbSearchOther.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbSearchOther.Location = new System.Drawing.Point(6, 93);
      this.cbSearchOther.Name = "cbSearchOther";
      this.cbSearchOther.Size = new System.Drawing.Size(113, 17);
      this.cbSearchOther.TabIndex = 4;
      this.cbSearchOther.Text = "Search &Other Text";
      this.cbSearchOther.UseVisualStyleBackColor = true;
      this.cbSearchOther.CheckedChanged += new System.EventHandler(this.FlagsChanged);
      // 
      // EnterSearchStringDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
      this.ClientSize = new System.Drawing.Size(292, 140);
      this.Controls.Add(this.cbSearchOther);
      this.Controls.Add(this.cbSearchComments);
      this.Controls.Add(this.cbSearchStrings);
      this.Controls.Add(this.cbCaseSensitive);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txtSearchString);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.btnCancel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(500, 198);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(300, 174);
      this.Name = "EnterSearchStringDialog";
      this.ShowInTaskbar = false;
      this.AcceptButton = btnOk;
      this.CancelButton = btnCancel;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Find Text in Solution";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.TextBox txtSearchString;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox cbCaseSensitive;
    private System.Windows.Forms.CheckBox cbSearchStrings;
    private System.Windows.Forms.CheckBox cbSearchComments;
    private System.Windows.Forms.CheckBox cbSearchOther;
  }
}