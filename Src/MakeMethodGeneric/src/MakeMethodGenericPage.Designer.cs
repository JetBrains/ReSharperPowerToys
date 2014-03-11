/*
 * Copyright 2007-2014 JetBrains
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

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{
  partial class MakeMethodGenericPage
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.myNameLabel = new System.Windows.Forms.Label();
      this.myTextName = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // myNameLabel
      // 
      this.myNameLabel.AutoSize = true;
      this.myNameLabel.Location = new System.Drawing.Point(4, 4);
      this.myNameLabel.Name = "myNameLabel";
      this.myNameLabel.Size = new System.Drawing.Size(123, 13);
      this.myNameLabel.TabIndex = 0;
      this.myNameLabel.Text = "Na&me of type parameter:";
      // 
      // myTextName
      // 
      this.myTextName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                     | System.Windows.Forms.AnchorStyles.Right)));
      this.myTextName.Location = new System.Drawing.Point(4, 21);
      this.myTextName.Name = "myTextName";
      this.myTextName.Size = new System.Drawing.Size(502, 20);
      this.myTextName.TabIndex = 1;
      // 
      // MakeMethodGenericPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.myTextName);
      this.Controls.Add(this.myNameLabel);
      this.Name = "MakeMethodGenericPage";
      this.Size = new System.Drawing.Size(509, 72);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label myNameLabel;
    private System.Windows.Forms.TextBox myTextName;
  }
}