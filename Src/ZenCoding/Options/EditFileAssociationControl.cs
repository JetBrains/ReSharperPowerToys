using System;

using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.UI.CommonControls;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public partial class EditFileAssociationControl : SafeUserControl
  {
    bool myUpdateCookie;

    public EditFileAssociationControl()
    {
    }

    public EditFileAssociationControl(FileAssociation fileAssociation)
    {
      InitializeComponent();

      SetUpValues(fileAssociation);
      SetUpValueChangedHandlers();
    }

    public FileAssociation FileAssociation { get; private set; }

    void SetUpValues(FileAssociation fileAssociation)
    {
      FileAssociation = fileAssociation;

      myPattern.Text = FileAssociation.Pattern;
      myEnabled.Checked = FileAssociation.Enabled;

      switch (FileAssociation.DocType)
      {
        case DocType.Html:
          myHtml.Checked = true;
          break;
        case DocType.Css:
          myCss.Checked = true;
          break;
        case DocType.Xsl:
          myXsl.Checked = true;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      switch (FileAssociation.PatternType)
      {
        case PatternType.FileExtension:
          myFileExtension.Checked = true;
          break;
        case PatternType.Regex:
          myRegex.Checked = true;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    void SetUpValueChangedHandlers()
    {
      myPattern.TextChanged += ParamsChanged;

      myFileExtension.CheckedChanged += ParamsChanged;
      myRegex.CheckedChanged += ParamsChanged;

      myHtml.CheckedChanged += ParamsChanged;
      myCss.CheckedChanged += ParamsChanged;
      myXsl.CheckedChanged += ParamsChanged;

      myEnabled.CheckedChanged += ParamsChanged;
    }

    void ParamsChanged(object sender, EventArgs e)
    {
      if (!myUpdateCookie)
      {
        myUpdateCookie = true;

        FileAssociation.Pattern = myPattern.Text;

        if (myFileExtension.Checked)
        {
          FileAssociation.PatternType = PatternType.FileExtension;
        }

        if (myRegex.Checked)
        {
          FileAssociation.PatternType = PatternType.Regex;
        }

        if (myHtml.Checked)
        {
          FileAssociation.DocType = DocType.Html;
        }

        if (myCss.Checked)
        {
          FileAssociation.DocType = DocType.Css;
        }

        if (myXsl.Checked)
        {
          FileAssociation.DocType = DocType.Xsl;
        }

        FileAssociation.Enabled = myEnabled.Checked;

        myUpdateCookie = false;
      }
    }
  }
}