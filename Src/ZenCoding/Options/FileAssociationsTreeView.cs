/*
 * Copyright 2007-2011 JetBrains
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
using JetBrains.ActionManagement;
using JetBrains.Application.Interop.NativeHook;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.TreeModels;
using JetBrains.UI.Application;
using JetBrains.UI.Components;
using JetBrains.UI.RichText;
using JetBrains.UI.Tooltips;
using JetBrains.UI.TreeView;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public class FileAssociationsTreeView : TreeModelPresentableView
  {
    TreeModelViewColumn myAssociationColumn;
    TreeModelViewColumn myPatternTypeColumn;

    public FileAssociationsTreeView(TreeModel model, ITreeViewController controller, IUIApplication environment, ITooltipManager tooltipManager, IWindowsHookManager windowsHookManager, IActionManager actionManager)
      : base(model, controller, environment, tooltipManager, windowsHookManager, actionManager)
    {
    }

    protected override void Initialize()
    {
      base.Initialize();
      ModelColumn.Name = "Pattern";
      ModelColumn.Caption = "Pattern";

      myPatternTypeColumn = AddColumn();
      myPatternTypeColumn.Name = "Pattern Type";
      myPatternTypeColumn.Caption = "Pattern Type";
      myPatternTypeColumn.Width = 50;

      myAssociationColumn = AddColumn();
      myAssociationColumn.Name = "Document Type";
      myAssociationColumn.Caption = "Document Type";
      myAssociationColumn.Width = 150;

      OptionsView.ShowColumns = true;
      OptionsView.ShowHorzLines = true;
      OptionsView.ShowRoot = false;
    }

    protected override void UpdateNodeCells(TreeModelViewNode viewNode, TreeModelNode modelNode, PresentationState state)
    {
      base.UpdateNodeCells(viewNode, modelNode, state);
      viewNode.SetValue(myPatternTypeColumn, RichText.Empty);
      viewNode.SetValue(myAssociationColumn, RichText.Empty);

      var association = modelNode.DataValue as FileAssociation;
      if (association != null)
      {
        UpdateNodeCellsForResult(viewNode, association);
      }
    }

    void UpdateNodeCellsForResult(TreeModelViewNode viewNode, FileAssociation fileAssociation)
    {
      viewNode.SetValue(myPatternTypeColumn, fileAssociation.PatternType);
      
      string docType = String.Format("{0}{1}", fileAssociation.DocType, fileAssociation.Enabled ? null : " (disabled)");
      viewNode.SetValue(myAssociationColumn, docType);
    }
  }
}
