using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using JetBrains.Annotations;
using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;
using JetBrains.UI.Options.OptionPages;
using JetBrains.UI.Avalon;

namespace JetBrains.ReSharper.PowerToys.Gist
{
  [OptionsPage(Pid, "Gist Settings", "gist", ParentId = EnvironmentPage.Pid)]
  public class GistOptionsPage : AOptionsPage
  {
    private readonly GitHubService myGitHubService;
    public const string Pid = "GistSettings";

    public GistOptionsPage([NotNull] Lifetime lifetime, SettingsStore settingsStore, GitHubService gitHubService)
      : base(lifetime, Pid, settingsStore)
    {
      if (lifetime == null)
        throw new ArgumentNullException("lifetime");

      myGitHubService = gitHubService;

      TextBox usernameBox;
      System.Windows.Forms.TextBox passwordBox;
      Control = InitView(out usernameBox, out passwordBox);

      Bind_GlobalContext<GitHubSettings, string>(s => s.Username, usernameBox, TextBox.TextProperty);
      Bind_GlobalContext<GitHubSettings, string>(s => s.Password, WinFormsProperty.Create(lifetime, passwordBox, box => box.Text, true));
    }

    private EitherControl InitView(out TextBox usernameBox, out System.Windows.Forms.TextBox passwordBox)
    {
      var grid = new Grid { Background = SystemColors.ControlBrush };

      var colDef1 = new ColumnDefinition { Width = GridLength.Auto };
      var colDef2 = new ColumnDefinition { Width = GridLength.Auto, MinWidth = 100 };
      var colDef3 = new ColumnDefinition { Width = GridLength.Auto, MinWidth = 100 };
      grid.ColumnDefinitions.Add(colDef1);
      grid.ColumnDefinitions.Add(colDef2);
      grid.ColumnDefinitions.Add(colDef3);

      // Define the Rows
      var rowDef1 = new RowDefinition { Height = GridLength.Auto };
      var rowDef2 = new RowDefinition { Height = GridLength.Auto };
      var rowDef3 = new RowDefinition { Height = GridLength.Auto };
      grid.RowDefinitions.Add(rowDef1);
      grid.RowDefinitions.Add(rowDef2);
      grid.RowDefinitions.Add(rowDef3);

      var header = new Label { Content = "GitHub access" };
      Grid.SetColumn(header, 0);
      Grid.SetColumnSpan(header, 3);
      Grid.SetRow(header, 0);

      var usernameLabel = new Label { Content = "Username:" };
      Grid.SetColumn(usernameLabel, 0);
      Grid.SetRow(usernameLabel, 1);

      usernameBox = new TextBox();
      Grid.SetColumn(usernameBox, 1);
      Grid.SetRow(usernameBox, 1);

      var passwordLabel = new Label { Content = "Password:" };
      Grid.SetColumn(passwordLabel, 0);
      Grid.SetRow(passwordLabel, 2);

      passwordBox = new System.Windows.Forms.TextBox { UseSystemPasswordChar = true };
      var passwordHost = new WindowsFormsHost { Child =  passwordBox };
      Grid.SetColumn(passwordHost, 1);
      Grid.SetRow(passwordHost, 2);

      grid.AddChild(header);
      grid.AddChild(usernameLabel);
      grid.AddChild(usernameBox);
      grid.AddChild(passwordLabel);
      grid.AddChild(passwordHost);

      return grid;
    }
  }
}