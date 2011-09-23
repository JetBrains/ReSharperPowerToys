using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.PowerToys.Gist.GitHub;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;
using JetBrains.UI.Options.OptionPages;
using JetBrains.UI.Avalon;
using RestSharp;
using JetBrains.Util.Special;

namespace JetBrains.ReSharper.PowerToys.Gist
{
  [OptionsPage(Pid, "Gist Settings", "gist", ParentId = EnvironmentPage.Pid, Sequence = 100)]
  public class GistOptionsPage : AOptionsPage
  {
    public const string Pid = "GistSettings";

    private const string DEFAULT_GRAVATAR = "https://gs1.wac.edgecastcdn.net/80460E/assets/images/gravatars/gravatar-140.png";
    private const int AVATAR_SIZE = 140;

    private readonly GitHubService myGitHubService;
    private readonly IDataContext myEmptyDataContext;

    public GistOptionsPage([NotNull] Lifetime lifetime, DataContexts dataContexts, GitHubService gitHubService, OptionsSettingsSmartContext settings)
      : base(lifetime, Pid)
    {
      if (lifetime == null)
        throw new ArgumentNullException("lifetime");

      myGitHubService = gitHubService;
      myEmptyDataContext = dataContexts.Empty;

      TextBox usernameBox;
      System.Windows.Forms.TextBox passwordBox;
      Control = InitView(out usernameBox, out passwordBox);

      settings.SetBinding(lifetime, (GitHubSettings s) => s.Username, usernameBox, TextBox.TextProperty);
      settings.SetBinding(lifetime, (GitHubSettings s) => s.Password, WinFormsProperty.Create(lifetime, passwordBox, box => box.Text, true));
    }

    private EitherControl InitView(out TextBox usernameBox, out System.Windows.Forms.TextBox passwordBox)
    {
      var grid = new Grid { Background = SystemColors.ControlBrush };

      var colDef1 = new ColumnDefinition { Width = GridLength.Auto };
      var colDef2 = new ColumnDefinition { Width = GridLength.Auto, MinWidth = AVATAR_SIZE };
      grid.ColumnDefinitions.Add(colDef1);
      grid.ColumnDefinitions.Add(colDef2);

      // Define the Rows
      var rowDef1 = new RowDefinition { Height = GridLength.Auto };
      var rowDef2 = new RowDefinition { Height = GridLength.Auto };
      var rowDef3 = new RowDefinition { Height = GridLength.Auto };
      var rowDef4 = new RowDefinition { Height = GridLength.Auto, MinHeight = AVATAR_SIZE, };
      grid.RowDefinitions.Add(rowDef1);
      grid.RowDefinitions.Add(rowDef2);
      grid.RowDefinitions.Add(rowDef3);
      grid.RowDefinitions.Add(rowDef4);

      var header = new Label { Content = "GitHub access" };
      Grid.SetColumn(header, 0);
      Grid.SetColumnSpan(header, 2);
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

      var imageBox = new Image { Width = AVATAR_SIZE, Margin = new Thickness(0, 10, 0, 10) };

      // Create source
      var image = new BitmapImage();
      // BitmapImage.UriSource must be in a BeginInit/EndInit block
      image.BeginInit();
      image.UriSource = new Uri(GetAvatar());
      image.DecodePixelWidth = AVATAR_SIZE;
      image.EndInit();
      //set image source
      imageBox.Source = image;
      Grid.SetColumn(imageBox, 1);
      Grid.SetRow(imageBox, 3);

      grid.AddChild(header);
      grid.AddChild(usernameLabel);
      grid.AddChild(usernameBox);
      grid.AddChild(passwordLabel);
      grid.AddChild(passwordHost);
      grid.AddChild(imageBox);

      return grid;
    }

    private string GetAvatar()
    {
      var response = myGitHubService.GetClient(myEmptyDataContext)
        .Execute<User>(new RestRequest("/user"));
      return response.Data.IfNotNull(_ => _.AvatarUrl) ?? DEFAULT_GRAVATAR;
    }
  }
}