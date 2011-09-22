using System.Drawing;
using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Browsing.TypeHierarchy;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.UI.Controls;
using JetBrains.UI.Extensions;
using JetBrains.UI.RichText;
using JetBrains.UI.ToolWindowManagement;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  [SolutionComponent]
  public class TypeInterfaceToolWindowRegistrar
  {
    private readonly Lifetime myLifetime;
    private readonly IShellLocks myLocks;
    private readonly SettingsStore mySettingsStore;
    private readonly IActionBarManager myActionBarManager;
    private readonly ToolWindowClass myToolWindowClass;

    public TypeInterfaceToolWindowRegistrar(Lifetime lifetime,
                                    ToolWindowManager toolWindowManager,
                                    IShellLocks locks,
                                    SettingsStore settingsStore,
                                    IActionManager actionManager,
                                    IActionBarManager actionBarManager,
                                    IShortcutManager shortcutManager,
                                    TypeInterfaceToolWindowDescriptor toolWindowDescriptor)
    {
      myLifetime = lifetime;
      myLocks = locks;
      mySettingsStore = settingsStore;
      myActionBarManager = actionBarManager;

      myToolWindowClass = toolWindowManager.Classes[toolWindowDescriptor];
      myToolWindowClass.RegisterEmptyContent(
        lifetime,
        lt =>
          {
            var emptyLabel = new RichTextLabel { BackColor = SystemColors.Control, Dock = DockStyle.Fill };
            emptyLabel.RichTextBlock.Add(new RichText("No hierarchies open", new TextStyle(FontStyle.Bold)));
            emptyLabel.RichTextBlock.Add(
              new RichText("Use " + actionManager.GetHowToExecuteAction(shortcutManager, typeof(BrowseTypeHierarchyAction)), TextStyle.Default));
            emptyLabel.RichTextBlock.Add(new RichText("on a type to see hierarchy", TextStyle.Default));
            emptyLabel.RichTextBlock.Parameters = new RichTextBlockParameters(8, ContentAlignment.MiddleCenter);
            return emptyLabel.BindToLifetime(lt);
          });
    }

    public void Show(TreeModelBrowserDescriptor browserDescriptor)
    {
      ToolWindowInstance instance = myToolWindowClass.RegisterInstance(
        myLifetime,
        StringUtil.MakeTitle(browserDescriptor.Title.Value), browserDescriptor.Image,
        (lt, twi) => TreeModelBrowserPanelPsiWPF.SelectTreeImplementation(browserDescriptor, lt, myActionBarManager, myLocks, mySettingsStore));
      instance.Lifetime.AddAction(() => browserDescriptor.LifetimeDefinition.Terminate());
      instance.EnsureControlCreated().Show();
    }
  }
}