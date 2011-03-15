using System.IO;
using System.Windows.Forms;
using JetBrains.Annotations;
using JetBrains.UI.CommonControls;
using JetBrains.UI.Options.Helpers;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  public class ReflectorOptionsPage : AStackPanelOptionsPage
  {
    private const string PID = "ReflectorNavigator.Options";

    public ReflectorOptionsPage()
      : base(PID)
    {
      InitControls();
    }

    private void InitControls()
    {
      ReflectorOptions options = ReflectorOptions.Instance;

      CheckBoxDisabledNoCheck check;
      Controls.Add(check = new CheckBoxDisabledNoCheck("&Reformat Reflector output with current settings"));
      Bind(options.PostReformat, check.CheckedLogicallyProperty);

      Controls.Add(check = new CheckBoxDisabledNoCheck("Show &XML doc if available"));
      Bind(options.ShowXmlDoc, check.CheckedLogicallyProperty);

      var label = new Label { Text = "Path to Reflector binary:", AutoSize = true };
      Controls.Add(label);

      TableLayoutPanel grid;
      Controls.Add(grid = UI.Options.Helpers.Controls.CreateGrid());
      grid.RowCount = 1;
      grid.ColumnCount = 2;
      grid.RowStyles.Insert(0, new RowStyle(SizeType.AutoSize));
      grid.ColumnStyles.Insert(0, new ColumnStyle(SizeType.Percent, 100));
      grid.ColumnStyles.Insert(1, new ColumnStyle(SizeType.AutoSize));

      grid.Controls.Add(new Controls.Separator(), 0, 0);
      var exeNameBox = new Controls.EditBox
                         {
                           ReadOnly = true,
                         };
      grid.Controls.Add(exeNameBox, 0, 1);
      Bind(options.ReflectorExe, exeNameBox.Text);

      var browseButton = new Controls.Button(
        "&Browse...",
        ()=>
          {
            exeNameBox.Text.Value = AskReflectorExePath(exeNameBox.Text.Value);
          });
      grid.Controls.Add(browseButton, 1, 1);

      Controls.Add(grid);
    }

    [CanBeNull]
    public static string AskReflectorExePath([CanBeNull] string currentPath)
    {
      using (var dialog = new OpenFileDialog())
      {
        dialog.CheckFileExists = true;
        dialog.AddExtension = true;
        dialog.DefaultExt = "Reflector.exe";
        dialog.Filter =
          "Reflector (Reflector.exe)|Reflector.exe|Exe files (*.exe)|*.exe";

        if (!string.IsNullOrEmpty(currentPath))
          dialog.FileName = currentPath;

        if (dialog.ShowDialog() != DialogResult.OK)
          return null;

        if (!File.Exists(dialog.FileName))
          return null;

        return dialog.FileName;
      }
    }
  }
}