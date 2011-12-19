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
using System.Linq.Expressions;
using System.Windows.Forms;
using JetBrains.Application.Settings.Store;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Common.Options;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.Threading;
using JetBrains.TreeModels;
using JetBrains.UI;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.Util;
using System.Linq;
using JetBrains.Application.Settings;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  [OptionsPage(ID, "Zen Coding", "zencoding", ParentId = ToolsPage.PID)]
  public partial class ZenCodingOptionsPage : UserControl, IOptionsPage
  {
    private const string ID = "ZenCoding-E439BB70-6F99-4C64-BA42-5D3DAEAC70E1";
    private readonly Lifetime myLifetime;
    private readonly OptionsSettingsSmartContext mySettings;
    private readonly IThreading myThreading;
    private readonly Dictionary<int, FileAssociation> myFileAssociations;

    private readonly FileAssociationsTreeView myView;
    private readonly Expression<Func<ZenCodingSettings, IIndexedEntry<int, FileAssociation>>> myLambdaExpression;

    public ZenCodingOptionsPage(Lifetime lifetime, OptionsSettingsSmartContext settings, IThreading threading)
    {
      myLifetime = lifetime;
      mySettings = settings;
      myThreading = threading;
      myLambdaExpression = s => s.FileAssociations;

      InitializeComponent();

      myFileAssociations = new Dictionary<int, FileAssociation>();
      mySettings.EnumerateIndexedEntry(myLambdaExpression).ForEach(pair => myFileAssociations[pair.First] = pair.Second);

      var indices = myFileAssociations.Keys.ToList();
      indices.Sort();
      var model = BuildModel(indices.Select(i => myFileAssociations[i]));

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
        var maxKey = myFileAssociations.Keys.Max();
        myFileAssociations.Add(maxKey + 1, form.FileAssociation);
        mySettings.SetIndexedValue(myLambdaExpression, maxKey + 1, form.FileAssociation);
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
          return;

        onClose(form);
        myThreading.ExecuteOrQueue(myLifetime, "ZenCodingOptionPage.UpdateAllNodesPresentation", () =>
        {
          myView.UpdateAllNodesPresentation();
        });
      }
    }

    private void RemoveFileAssociation(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      var pairToRemove = myFileAssociations.First(pair => pair.Value == selection);
      myFileAssociations.Remove(pairToRemove.Key);
      mySettings.RemoveIndexedValue(myLambdaExpression, pairToRemove.Key);

      BindModel(null);
    }

    private void MoveUp(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      var indices = myFileAssociations.Keys.ToList();
      indices.Sort();

      for (int i = 0; i < indices.Count; i++)
      {
        var association = myFileAssociations[indices[i]];
        if (ReferenceEquals(selection, association) && i > 0)
        {
          Exchange(indices[i], indices[i - 1]);
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

      var indices = myFileAssociations.Keys.ToList();
      indices.Sort();

      for (int i = 0; i < indices.Count; i++)
      {
        var association = myFileAssociations[indices[i]];
        if (ReferenceEquals(selection, association) && i + 1 < indices.Count)
        {
          Exchange(indices[i], indices[i + 1]);
          BindModel(selection);
          break;
        }
      }
    }

    private void Exchange(int first, int second)
    {
      var temp = myFileAssociations[first];
      myFileAssociations[first] = myFileAssociations[second];
      myFileAssociations[second] = temp;

      mySettings.RemoveIndexedValue(myLambdaExpression, first);
      mySettings.SetIndexedValue(myLambdaExpression, first, myFileAssociations[first]);
      mySettings.RemoveIndexedValue(myLambdaExpression, second);
      mySettings.SetIndexedValue(myLambdaExpression, second, myFileAssociations[second]);
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

      mySettings.ResetIndexedValues(myLambdaExpression);
      myFileAssociations.Clear();

      for (int i = 0; i < fileAssociaitons.Count; i++)
      {
        var associaiton = fileAssociaitons[i];
        myFileAssociations[i] = associaiton;
        mySettings.SetIndexedValue(myLambdaExpression, i, associaiton);
      }

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
      myThreading.ExecuteOrQueue(myLifetime, "ZenCodingOptionsPage.BindModel", () =>
      {
        var indices = myFileAssociations.Keys.ToList();
        indices.Sort();

        myView.Model = BuildModel(indices.Select(i => myFileAssociations[i]));
        myView.UpdateAllNodesPresentation();

        foreach (var pair in myFileAssociations)
        {
          var association = pair.Value;
          if (ReferenceEquals(selection, association))
          {
            myView.SetFocusedNode(myView.FindNodeByID(pair.Key));
            break;
          }
        }
      });
    }
  }
}