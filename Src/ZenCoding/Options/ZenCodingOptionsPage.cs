using System;
using System.Collections.Generic;
using System.Windows.Forms;

using JetBrains.ReSharper.Features.Common.Options;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.TreeModels;
using JetBrains.UI;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  [OptionsPage(ID,
    "Zen Coding",
    "zencoding",
    ParentId = ToolsPage.PID)]
  public partial class ZenCodingOptionsPage : UserControl, IOptionsPage
  {
    const string ID = "ZenCoding-E439BB70-6F99-4C64-BA42-5D3DAEAC70E1";
    readonly FileAssociationsTreeView myView;

    public ZenCodingOptionsPage()
    {
      InitializeComponent();

      var model = BuildModel(Settings.Instance.FileAssociations);
      myView = new FileAssociationsTreeView(model, new FileAssociationViewController())
      {
        Presenter = new FileAssociationPresenter(),
        Dock = DockStyle.Fill
      };
      myView.DoubleClick += EditFileAssociation;
      myRules.Controls.Add(myView);

      _buttons.Items.Add("Create", ImageLoader.GetImage("Create"), CreateFileAssociation);

      _buttons.Items.Add("Edit", ImageLoader.GetImage("Edit"), EditFileAssociation);

      _buttons.Items.Add("Remove", ImageLoader.GetImage("Remove"), RemoveFileAssociation);

      _buttons.Items.Add("Up", ImageLoader.GetImage("Up"), MoveUp);

      _buttons.Items.Add("Down", ImageLoader.GetImage("Down"), MoveDown);
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

    void CreateFileAssociation(object sender, EventArgs e)
    {
      OpenEditor(new FileAssociation(), form =>
      {
        Settings.Instance.FileAssociations.Add(form.FileAssociation);
        BindModel(null);
      });
    }

    void EditFileAssociation(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      OpenEditor((FileAssociation) selection.Clone(), form => selection.CopyFrom(form.FileAssociation));
    }

    void OpenEditor(FileAssociation association, Action<EditFileAssociationForm> onClose)
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

    void RemoveFileAssociation(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      Settings.Instance.FileAssociations.Remove(selection);

      BindModel(null);
    }

    void MoveUp(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      for (int i = 0; i < Settings.Instance.FileAssociations.Count; i++)
      {
        var association = Settings.Instance.FileAssociations[i];
        if (ReferenceEquals(selection, association) && i > 0)
        {
          Exchange(i, i - 1);
          BindModel(selection);
          break;
        }
      }
    }

    void MoveDown(object sender, EventArgs e)
    {
      FileAssociation selection = GetSelectedFileAssociation();
      if (selection == null)
      {
        return;
      }

      for (int i = 0; i < Settings.Instance.FileAssociations.Count; i++)
      {
        var association = Settings.Instance.FileAssociations[i];
        if (ReferenceEquals(selection, association) && i + 1 < Settings.Instance.FileAssociations.Count)
        {
          Exchange(i, i + 1);
          BindModel(selection);
          break;
        }
      }
    }

    static void Exchange(int first, int second)
    {
      var temp = Settings.Instance.FileAssociations[first];
      Settings.Instance.FileAssociations[first] = Settings.Instance.FileAssociations[second];
      Settings.Instance.FileAssociations[second] = temp;
    }

    FileAssociation GetSelectedFileAssociation()
    {
      TreeModelNode selection = myView.ModelFocusedNode;
      if (selection == null)
      {
        return null;
      }

      var association = selection.DataValue as FileAssociation;
      if (association == null)
      {
        return association;
      }
      return association;
    }

    void ResetClick(object sender, EventArgs e)
    {
      Settings.Instance.FileAssociations = new List<FileAssociation>();

      foreach (var association in Settings.Default.FileAssociations)
      {
        Settings.Instance.FileAssociations.Add((FileAssociation) association.Clone());
      }

      BindModel(null);
    }

    static TreeSimpleModel BuildModel(IEnumerable<FileAssociation> fileAssociations)
    {
      var model = new TreeSimpleModel();

      foreach (var association in fileAssociations ?? EmptyArray<FileAssociation>.Instance)
      {
        model.Insert(null, association);
      }

      return model;
    }

    void BindModel(FileAssociation selection)
    {
      myView.Model = BuildModel(Settings.Instance.FileAssociations);
      myView.UpdateAllNodesPresentation();

      for (int i = 0; i < Settings.Instance.FileAssociations.Count; i++)
      {
        var association = Settings.Instance.FileAssociations[i];
        if (ReferenceEquals(selection, association))
        {
          myView.SetFocusedNode(myView.FindNodeByID(i));
          break;
        }
      }
    }
  }
}