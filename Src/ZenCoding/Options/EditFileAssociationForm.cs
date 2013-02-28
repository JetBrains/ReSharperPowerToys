﻿/*
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

using System;
using System.Windows.Forms;

using JetBrains.Application;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.UI.Components;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public partial class EditFileAssociationForm : Form
  {
    readonly EditFileAssociationControl myEditor;

    public EditFileAssociationForm(FileAssociation fileAssociation, UIApplicationEnvironment environment)
    {
      InitializeComponent();

      myEditor = new EditFileAssociationControl(fileAssociation, environment)
      {
        Dock = DockStyle.Fill
      };

      myPanel.Controls.Add(myEditor);

      Icon = Shell.Instance.GetComponent<IApplicationDescriptor>().ProductIcon;
    }

    public FileAssociation FileAssociation
    {
      get { return myEditor.FileAssociation; }
    }

    protected override void OnClosed(EventArgs e)
    {
    }
  }
}