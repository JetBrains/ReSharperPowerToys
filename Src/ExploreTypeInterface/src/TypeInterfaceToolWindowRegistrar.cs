/*
 * Copyright 2007-2014 JetBrains
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Drawing;
using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.Interop.NativeHook;
using JetBrains.DataFlow;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Occurences.Presentation.TreePsiBrowser;
using JetBrains.ReSharper.Features.Browsing.Hierarchies.Actions;
using JetBrains.UI.Application;
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
    private readonly Lifetime _lifetime;
    private readonly IActionBarManager _actionBarManager;
    private readonly ToolWindowClass _toolWindowClass;
    private readonly IUIApplication _environment;
    private readonly IWindowsHookManager _windowsHookManager;

    public TypeInterfaceToolWindowRegistrar(Lifetime lifetime,
                                    ToolWindowManager toolWindowManager,
                                    IActionManager actionManager,
                                    IActionBarManager actionBarManager,
                                    IShortcutManager shortcutManager,
                                    TypeInterfaceToolWindowDescriptor toolWindowDescriptor,
                                    IUIApplication environment,
                                    IWindowsHookManager windowsHookManager)
    {
      _lifetime = lifetime;
      _actionBarManager = actionBarManager;
      _environment = environment;
      _windowsHookManager = windowsHookManager;

      _toolWindowClass = toolWindowManager.Classes[toolWindowDescriptor];
      _toolWindowClass.RegisterEmptyContent(
        lifetime,
        lt =>
          {
            var emptyLabel = new RichTextLabel(environment) { BackColor = SystemColors.Control, Dock = DockStyle.Fill };
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
      ToolWindowInstance instance = _toolWindowClass.RegisterInstance(
        _lifetime,
        StringUtil.MakeTitle(browserDescriptor.Title.Value), browserDescriptor.Image,
        (lt, twi) => new TreeModelBrowserPanelPsiWPF(browserDescriptor, lt, _actionBarManager, _environment, _windowsHookManager));
      instance.Lifetime.AddAction(() => browserDescriptor.LifetimeDefinition.Terminate());
      instance.EnsureControlCreated().Show();
    }
  }
}