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
  partial class EditFileAssociationControl
  {
    CheckBox myEnabled;
    TextBox myPattern;
    
    RadioButton myFileExtension;
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
      System.Windows.Forms.Label _patternLabel;
      System.Windows.Forms.Label _patternTypeLabel;
      System.Windows.Forms.Label _associationLabel;
      this.myFileExtension = new System.Windows.Forms.RadioButton();
      this.myPattern = new System.Windows.Forms.TextBox();
      this.myEnabled = new System.Windows.Forms.CheckBox();
      this.myRegex = new System.Windows.Forms.RadioButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.myXsl = new System.Windows.Forms.RadioButton();
      this.myCss = new System.Windows.Forms.RadioButton();
      this.myHtml = new System.Windows.Forms.RadioButton();
      this.panel2 = new System.Windows.Forms.Panel();
      _patternLabel = new System.Windows.Forms.Label();
      _patternTypeLabel = new System.Windows.Forms.Label();
      _associationLabel = new System.Windows.Forms.Label();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // _patternLabel
      // 
      _patternLabel.AutoSize = true;
      _patternLabel.Location = new System.Drawing.Point(3, 6);
      _patternLabel.Name = "_patternLabel";
      _patternLabel.Size = new System.Drawing.Size(44, 13);
      _patternLabel.TabIndex = 0;
      _patternLabel.Text = "&Pattern:";
      // 
      // _patternTypeLabel
      // 
      _patternTypeLabel.AutoSize = true;
      _patternTypeLabel.Location = new System.Drawing.Point(4, 5);
      _patternTypeLabel.Name = "_patternTypeLabel";
      _patternTypeLabel.Size = new System.Drawing.Size(71, 13);
      _patternTypeLabel.TabIndex = 0;
      _patternTypeLabel.Text = "Pattern &Type:";
      // 
      // _associationLabel
      // 
      _associationLabel.AutoSize = true;
      _associationLabel.Location = new System.Drawing.Point(4, 6);
      _associationLabel.Name = "_associationLabel";
      _associationLabel.Size = new System.Drawing.Size(64, 13);
      _associationLabel.TabIndex = 0;
      _associationLabel.Text = "&Association:";
      // 
      // _fileExtension
      // 
      this.myFileExtension.AutoSize = true;
      this.myFileExtension.Location = new System.Drawing.Point(94, 3);
      this.myFileExtension.Name = "_fileExtension";
      this.myFileExtension.Size = new System.Drawing.Size(121, 17);
      this.myFileExtension.TabIndex = 1;
      this.myFileExtension.TabStop = true;
      this.myFileExtension.Text = "File E&xtension (.html)";
      this.myFileExtension.UseVisualStyleBackColor = true;
      // 
      // _pattern
      // 
      this.myPattern.Location = new System.Drawing.Point(96, 3);
      this.myPattern.Name = "_pattern";
      this.myPattern.Size = new System.Drawing.Size(538, 20);
      this.myPattern.TabIndex = 1;
      // 
      // _enabled
      // 
      this.myEnabled.AutoSize = true;
      this.myEnabled.Location = new System.Drawing.Point(3, 172);
      this.myEnabled.Name = "_enabled";
      this.myEnabled.Size = new System.Drawing.Size(95, 17);
      this.myEnabled.TabIndex = 4;
      this.myEnabled.Text = "&Enable pattern";
      this.myEnabled.UseVisualStyleBackColor = true;
      // 
      // _regex
      // 
      this.myRegex.AutoSize = true;
      this.myRegex.Location = new System.Drawing.Point(94, 26);
      this.myRegex.Name = "_regex";
      this.myRegex.Size = new System.Drawing.Size(159, 17);
      this.myRegex.TabIndex = 2;
      this.myRegex.TabStop = true;
      this.myRegex.Text = "&Regular Expression (.*\\.html)";
      this.myRegex.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.myXsl);
      this.panel1.Controls.Add(this.myCss);
      this.panel1.Controls.Add(this.myHtml);
      this.panel1.Controls.Add(_associationLabel);
      this.panel1.Location = new System.Drawing.Point(0, 83);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(262, 83);
      this.panel1.TabIndex = 3;
      // 
      // _xsl
      // 
      this.myXsl.AutoSize = true;
      this.myXsl.Location = new System.Drawing.Point(94, 50);
      this.myXsl.Name = "_xsl";
      this.myXsl.Size = new System.Drawing.Size(45, 17);
      this.myXsl.TabIndex = 3;
      this.myXsl.TabStop = true;
      this.myXsl.Text = "X&SL";
      this.myXsl.UseVisualStyleBackColor = true;
      // 
      // _css
      // 
      this.myCss.AutoSize = true;
      this.myCss.Location = new System.Drawing.Point(94, 27);
      this.myCss.Name = "_css";
      this.myCss.Size = new System.Drawing.Size(46, 17);
      this.myCss.TabIndex = 2;
      this.myCss.TabStop = true;
      this.myCss.Text = "&CSS";
      this.myCss.UseVisualStyleBackColor = true;
      // 
      // _html
      // 
      this.myHtml.AutoSize = true;
      this.myHtml.Location = new System.Drawing.Point(94, 4);
      this.myHtml.Name = "_html";
      this.myHtml.Size = new System.Drawing.Size(55, 17);
      this.myHtml.TabIndex = 1;
      this.myHtml.TabStop = true;
      this.myHtml.Text = "&HTML";
      this.myHtml.UseVisualStyleBackColor = true;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(_patternTypeLabel);
      this.panel2.Controls.Add(this.myFileExtension);
      this.panel2.Controls.Add(this.myRegex);
      this.panel2.Location = new System.Drawing.Point(0, 29);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(263, 48);
      this.panel2.TabIndex = 2;
      // 
      // EditFileAssociationControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.myEnabled);
      this.Controls.Add(this.myPattern);
      this.Controls.Add(_patternLabel);
      this.Name = "EditFileAssociationControl";
      this.Size = new System.Drawing.Size(269, 199);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private RadioButton myRegex;
    private Panel panel1;
    private RadioButton myXsl;
    private RadioButton myCss;
    private RadioButton myHtml;
    private Panel panel2;
  }
}
