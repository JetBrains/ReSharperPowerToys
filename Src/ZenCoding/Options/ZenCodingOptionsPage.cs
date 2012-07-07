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
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Icons;
using JetBrains.UI.Options;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.UI.Resources;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  [OptionsPage(ID, "Zen Coding", typeof(ZenCodingThemedIcons.Zencoding), ParentId = ToolsPage.PID)]
  public partial class ZenCodingOptionsPage : UserControl, IOptionsPage
  {
    private const string ID = "ZenCoding-E439BB70-6F99-4C64-BA42-5D3DAEAC70E1";
    private readonly Lifetime myLifetime;
    private readonly OptionsSettingsSmartContext mySettings;
    private readonly IThreading myThreading;
    private readonly IThemedIconManager myIconManager;
    private readonly SortedDictionary<int, FileAssociation> myFileAssociations;

    private readonly FileAssociationsTreeView myView;
    private readonly Expression<Func<ZenCodingSettings, IIndexedEntry<int, FileAssociation>>> myLambdaExpression;

    public ZenCodingOptionsPage(Lifetime lifetime, OptionsSettingsSmartContext settings, IThreading threading, IThemedIconManager iconManager)
    {
      myLifetime = lifetime;
      mySettings = settings;
      myThreading = threading;
      myIconManager = iconManager;
      myLambdaExpression = s => s.FileAssociations;

      InitializeComponent();

      myFileAssociations = new SortedDictionary<int, FileAssociation>();
      foreach (var pair in mySettings.EnumerateIndexedEntry(myLambdaExpression))
      {
        myFileAssociations[pair.First] = pair.Second;
      }

      var model = BuildModel();

      myView = new FileAssociationsTreeView(model, new FileAssociationViewController())
      {
        Presenter = new FileAssociationPresenter(),
        Dock = DockStyle.Fill
      };
      myView.DoubleClick += EditFileAssociation;
      myRules.Controls.Add(myView);

      _buttons.Items.Add("Create", myIconManager.Icons[ZenCodingCommonThemedIcons.Add.Id].CurrentGdipBitmap96, CreateFileAssociation);
      _buttons.Items.Add("Edit", myIconManager.Icons[CommonThemedIcons.Edit.Id].CurrentGdipBitmap96, EditFileAssociation);
      _buttons.Items.Add("Remove", myIconManager.Icons[CommonThemedIcons.Remove.Id].CurrentGdipBitmap96, RemoveFileAssociation);
      _buttons.Items.Add("Up", myIconManager.Icons[CommonThemedIcons.Up.Id].CurrentGdipBitmap96, MoveUp);
      _buttons.Items.Add("Down", myIconManager.Icons[CommonThemedIcons.Down.Id].CurrentGdipBitmap96, MoveDown);
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
        var nextKey = myFileAssociations.Keys.Max() + 1;
        myFileAssociations[nextKey] = form.FileAssociation;
        mySettings.SetIndexedValue(myLambdaExpression, nextKey, form.FileAssociation);
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

      OpenEditor((FileAssociation) selection.Clone(), form =>
      {
        selection.CopyFrom(form.FileAssociation);
        mySettings.SetIndexedValue(myLambdaExpression, myFileAssociations.Where(pair => pair.Value == selection).Select(_ => _.Key).First(), selection);
      });
    }

    private void OpenEditor(FileAssociation association, Action<EditFileAssociationForm> onClose)
    {
      using (var form = new EditFileAssociationForm(association))
      {
        if (form.ShowDialog(this) != DialogResult.OK)
          return;

        onClose(form);
        myThreading.ExecuteOrQueue(myLifetime, "ZenCodingOptionPage.UpdateAllNodesPresentation", () => myView.UpdateAllNodesPresentation());
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

    private TreeSimpleModel BuildModel()
    {
      var model = new TreeSimpleModel();

      var indices = myFileAssociations.Keys.ToList();

      foreach (var association in indices.Select(i => myFileAssociations[i]))
      {
        model.Insert(null, association);
      }

      return model;
    }

    private void BindModel(FileAssociation selection)
    {
      myThreading.ExecuteOrQueue(myLifetime, "ZenCodingOptionsPage.BindModel", () =>
      {
        myView.Model = BuildModel();
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