using System;

using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.TreeModels;
using JetBrains.UI.RichText;
using JetBrains.UI.TreeView;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public class FileAssociationsTreeView : TreeModelPresentableView
  {
    TreeModelViewColumn myAssociationColumn;
    TreeModelViewColumn myPatternTypeColumn;

    public FileAssociationsTreeView(TreeModel model, ITreeViewController controller) : base(model, controller)
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
