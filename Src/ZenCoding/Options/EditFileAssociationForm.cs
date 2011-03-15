using System;
using System.Windows.Forms;

using JetBrains.Application;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public partial class EditFileAssociationForm : Form
  {
    readonly EditFileAssociationControl myEditor;

    public EditFileAssociationForm(FileAssociation fileAssociation)
    {
      InitializeComponent();

      myEditor = new EditFileAssociationControl(fileAssociation)
      {
        Dock = DockStyle.Fill
      };

      myPanel.Controls.Add(myEditor);

      Icon = Shell.Instance.Descriptor.ProductIcon;
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