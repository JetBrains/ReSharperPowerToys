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

using System.Windows.Forms;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  partial class ZenCodingOptionsPage
  {
    ToolStrip _buttons;

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
      System.Windows.Forms.Panel _fileAssociations;
      System.Windows.Forms.Panel _container;
      this.myRules = new System.Windows.Forms.Panel();
      this._buttons = new System.Windows.Forms.ToolStrip();
      _fileAssociations = new System.Windows.Forms.Panel();
      _container = new System.Windows.Forms.Panel();
      _fileAssociations.SuspendLayout();
      _container.SuspendLayout();
      this.SuspendLayout();
      // 
      // _fileAssociations
      // 
      _fileAssociations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      _fileAssociations.Controls.Add(this.myRules);
      _fileAssociations.Controls.Add(this._buttons);
      _fileAssociations.Location = new System.Drawing.Point(3, 3);
      _fileAssociations.Name = "_fileAssociations";
      _fileAssociations.Size = new System.Drawing.Size(767, 696);
      _fileAssociations.TabIndex = 12;
      // 
      // _rules
      // 
      this.myRules.Dock = System.Windows.Forms.DockStyle.Fill;
      this.myRules.Location = new System.Drawing.Point(0, 25);
      this.myRules.Name = "_rules";
      this.myRules.Size = new System.Drawing.Size(767, 671);
      this.myRules.TabIndex = 1;
      // 
      // _buttons
      // 
      this._buttons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this._buttons.Location = new System.Drawing.Point(0, 0);
      this._buttons.Name = "_buttons";
      this._buttons.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this._buttons.Size = new System.Drawing.Size(767, 25);
      this._buttons.TabIndex = 0;
      this._buttons.Text = "_buttons";
      // 
      // _container
      // 
      _container.Controls.Add(_fileAssociations);
      _container.Dock = System.Windows.Forms.DockStyle.Fill;
      _container.Location = new System.Drawing.Point(0, 0);
      _container.Name = "_container";
      _container.Size = new System.Drawing.Size(773, 731);
      _container.TabIndex = 0;
      // 
      // ZenCodingOptionsPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(_container);
      this.Name = "ZenCodingOptionsPage";
      this.Size = new System.Drawing.Size(773, 731);
      _fileAssociations.ResumeLayout(false);
      _fileAssociations.PerformLayout();
      _container.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private Panel myRules;
  }
}