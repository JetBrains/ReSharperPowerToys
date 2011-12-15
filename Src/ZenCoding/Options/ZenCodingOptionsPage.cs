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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Common.Options;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.TreeModels;
using JetBrains.UI;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.Util;
using JetBrains.Application.Settings;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  [OptionsPage(ID, "Zen Coding", "zencoding", ParentId = ToolsPage.PID)]
  public partial class ZenCodingOptionsPage : UserControl, IOptionsPage
  {
    const string ID = "ZenCoding-E439BB70-6F99-4C64-BA42-5D3DAEAC70E1";
    
    readonly FileAssociationsTreeView myView;
    private readonly IProperty<List<FileAssociation>> myFileAssociations;

    public ZenCodingOptionsPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
    {
      InitializeComponent();

      myFileAssociations = settings.GetValueProperty(lifetime, (ZenCodingSettings zcs) => zcs.FileAssociations);
      var model = BuildModel(myFileAssociations.Value);
      myView = new FileAssociationsTreeView(model, new FileAssociationViewController())
      {
        Presenter = new FileAssociationPresenter(),
        Dock = DockStyle.Fill
      };
      myView.DoubleClick += EditFileAssociation;
      myRules.Controls.Add(myView);

      var assembly = GetType().Assembly;

      _buttons.Items.Add("Create", ImageLoader.GetImage("Create", assembly), CreateFileAssociation);

      _buttons.Items.Add("Edit", ImageLoader.GetImage("Edit", assembly), EditFileAssociation);

      _buttons.Items.Add("Remove", ImageLoader.GetImage("Remove", assembly), RemoveFileAssociation);

      _buttons.Items.Add("Up", ImageLoader.GetImage("Up", assembly), MoveUp);

      _buttons.Items.Add("Down", ImageLoader.GetImage("Down", assembly), MoveDown);
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
      return true;
    }

    public bool ValidatePage()
    {
      return true;
    }

    private void CreateFileAssociation(object sender, EventArgs e)
    {
      OpenEditor(new FileAssociation(), form =>
      {
        myFileAssociations.Value.Add(form.FileAssociation);
        BindModel(null);
      });
    }

    private void EditFileAssociation(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      OpenEditor((FileAssociation) selection.Clone(), form => selection.CopyFrom(form.FileAssociation));
    }

    private void OpenEditor(FileAssociation association, Action<EditFileAssociationForm> onClose)
    {
      using (var form = new EditFileAssociationForm(association))
      {
        if (form.ShowDialog(this) != DialogResult.OK)
        {
          return;
        }

        onClose(form);
        myView.UpdateAllNodesPresentation();
      }
    }

    private void RemoveFileAssociation(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      myFileAssociations.Value.Remove(selection);

      BindModel(null);
    }

    private void MoveUp(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      var fileAssociations = myFileAssociations.Value;
      for (int i = 0; i < fileAssociations.Count; i++)
      {
        var association = fileAssociations[i];
        if (ReferenceEquals(selection, association) && i > 0)
        {
          Exchange(i, i - 1);
          BindModel(selection);
          break;
        }
      }
    }

    private void MoveDown(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      var fileAssociations = myFileAssociations.Value;
      for (int i = 0; i < fileAssociations.Count; i++)
      {
        var association = fileAssociations[i];
        if (ReferenceEquals(selection, association) && i + 1 < fileAssociations.Count)
        {
          Exchange(i, i + 1);
          BindModel(selection);
          break;
        }
      }
    }

    private void Exchange(int first, int second)
    {
      var fileAssociations = myFileAssociations.Value;
      var temp = fileAssociations[first];
      fileAssociations[first] = fileAssociations[second];
      fileAssociations[second] = temp;
    }

    private FileAssociation GetSelectedFileAssociation()
    {
      TreeModelNode selection = myView.ModelFocusedNode;
      if (selection == null)
        return null;

      return selection.DataValue as FileAssociation;
    }

    private void ResetClick(object sender, EventArgs e)
    {
      // TODO: redo with settings reset API
      var fileAssociaitons = new List<FileAssociation>();

      foreach (var association in Model.Settings.Default.FileAssociations)
      {
        fileAssociaitons.Add((FileAssociation) association.Clone());
      }
      myFileAssociations.Value = fileAssociaitons;

      BindModel(null);
    }

    private static TreeSimpleModel BuildModel(IEnumerable<FileAssociation> fileAssociations)
    {
      var model = new TreeSimpleModel();

      foreach (var association in fileAssociations ?? EmptyArray<FileAssociation>.Instance)
      {
        model.Insert(null, association);
      }

      return model;
    }

    private void BindModel(FileAssociation selection)
    {
      var fileAssociations = myFileAssociations.Value;
      myView.Model = BuildModel(fileAssociations);
      myView.UpdateAllNodesPresentation();

      for (int i = 0; i < fileAssociations.Count; i++)
      {
        var association = fileAssociations[i];
        if (ReferenceEquals(selection, association))
        {
          myView.SetFocusedNode(myView.FindNodeByID(i));
          break;
        }
      }
    }
  }
}