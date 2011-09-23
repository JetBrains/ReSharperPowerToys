using System;
using System.ComponentModel;
using System.Windows.Forms;
using JetBrains.Application.Configuration;
using JetBrains.Application.DataContext;
using JetBrains.Application.src.Settings;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  public partial class EnterSearchStringDialog : Form
  {
    private readonly IContextBoundSettingsStore2 mySettingsStore;

    public EnterSearchStringDialog(IContextBoundSettingsStore2 settingsStore)
    {
      mySettingsStore = settingsStore;
      InitializeComponent();
      
      // Gettings previously saved state or default values from settings
      string searchString = mySettingsStore.GetValue((FindTextSettings s) => s.LastUsedText) ?? string.Empty;

      var searchFlags = mySettingsStore.GetValue((FindTextSettings s) => s.LastUsedFlags);
      if (searchFlags == 0)
        searchFlags = FindTextSearchFlags.All;

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
      mySettingsStore.SetValue((FindTextSettings s) => s.LastUsedText, SearchString);
      mySettingsStore.SetValue((FindTextSettings s) => s.LastUsedFlags, SearchFlags);
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