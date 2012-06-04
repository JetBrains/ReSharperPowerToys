/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Drawing;
using System.Windows.Forms;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  partial class EditFileAssociationForm
  {
    Panel myPanel;

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
      System.Windows.Forms.Button _cancel;
      System.Windows.Forms.Button _ok;
      this.myPanel = new System.Windows.Forms.Panel();
      _cancel = new System.Windows.Forms.Button();
      _ok = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _cancel
      // 
      _cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      _cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      _cancel.Location = new System.Drawing.Point(297, 227);
      _cancel.Name = "_cancel";
      _cancel.Size = new System.Drawing.Size(75, 23);
      _cancel.TabIndex = 11;
      _cancel.Text = "&Cancel";
      _cancel.UseVisualStyleBackColor = true;
      // 
      // _ok
      // 
      _ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      _ok.DialogResult = System.Windows.Forms.DialogResult.OK;
      _ok.Location = new System.Drawing.Point(216, 227);
      _ok.Name = "_ok";
      _ok.Size = new System.Drawing.Size(75, 23);
      _ok.TabIndex = 10;
      _ok.Text = "&OK";
      _ok.UseVisualStyleBackColor = true;
      // 
      // _panel
      // 
      this.myPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.myPanel.Location = new System.Drawing.Point(12, 3);
      this.myPanel.Name = "_panel";
      this.myPanel.Size = new System.Drawing.Size(360, 218);
      this.myPanel.TabIndex = 12;
      // 
      // EditFileAssociationForm
      // 
      this.AcceptButton = _ok;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = _cancel;
      this.ClientSize = new System.Drawing.Size(384, 262);
      this.Controls.Add(this.myPanel);
      this.Controls.Add(_ok);
      this.Controls.Add(_cancel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(400, 300);
      this.Name = "EditFileAssociationForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Edit File Association...";
      this.ResumeLayout(false);

    }

    #endregion
  }
}