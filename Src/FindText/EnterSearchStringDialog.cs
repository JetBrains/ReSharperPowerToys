using System;
using System.ComponentModel;
using System.Windows.Forms;
using JetBrains.Application;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  public partial class EnterSearchStringDialog : Form
  {
    public EnterSearchStringDialog()
    {
      InitializeComponent();
      
      // Gettings previously saved state or default values from global settings
      string searchString = GlobalSettingsTable.Instance.GetString("jetbrains.resharper.powertoy.findtext.recenttext", "");
      var searchFlags = (FindTextSearchFlags) GlobalSettingsTable.Instance.GetInteger("jetbrains.resharper.powertoy.findtext.recentflags", (int)FindTextSearchFlags.All);
      txtSearchString.Text = searchString;
      txtSearchString.SelectAll();

      if ((searchFlags & FindTextSearchFlags.StringLiterals) != FindTextSearchFlags.None)
        cbSearchStrings.Checked = true;
      if ((searchFlags & FindTextSearchFlags.Comments) != FindTextSearchFlags.None)
        cbSearchComments.Checked = true;
      if ((searchFlags & FindTextSearchFlags.Other) != FindTextSearchFlags.None)
        cbSearchOther.Checked = true;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      // Saving state to global settings
      GlobalSettingsTable.Instance.SetString("jetbrains.resharper.powertoy.findtext.recenttext", SearchString);
      GlobalSettingsTable.Instance.SetInteger("jetbrains.resharper.powertoy.findtext.recentflags", (int) SearchFlags);
    }

    public string SearchString
    {
      get { return txtSearchString.Text; }
    }

    public bool CaseSensitive
    {
      get { return cbCaseSensitive.Checked; }
    }

    public FindTextSearchFlags SearchFlags
    {
      get
      {
        FindTextSearchFlags flags = FindTextSearchFlags.None;
        if (cbSearchStrings.Checked)
          flags |= FindTextSearchFlags.StringLiterals;
        if (cbSearchComments.Checked)
          flags |= FindTextSearchFlags.Comments;
        if (cbSearchOther.Checked)
          flags |= FindTextSearchFlags.Other;
        return flags;
      }
    }

    private void FlagsChanged(object sender, EventArgs e)
    {
      btnOk.Enabled = SearchFlags != FindTextSearchFlags.None && txtSearchString.Text.Length > 0;
    }
  }
}