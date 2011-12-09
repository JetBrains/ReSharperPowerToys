/*
 * Copyright 2007-2011 JetBrains s.r.o.
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

using System.Windows.Controls.Primitives;
using JetBrains.Application.src.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Common.Options;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace JetBrains.ReSharper.PowerToys.OptionsPage
{
  [OptionsPage(PID, "Sample Page", "JetBrains.ReSharper.PowerToys.OptionsPage.samplePage.gif", ParentId = ToolsPage.PID)]
  public partial class SampleOptionPage : IOptionsPage
  {
    private const string PID = "SamplePageId";

    public SampleOptionPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
    {
      InitializeComponent();
      settings.SetBinding(lifetime, (SampleSettings s) => s.SampleOption, mySampleOptionCheckBox, ToggleButton.IsCheckedProperty);
    }

    public EitherControl Control
    {
      get { return this; }
    }

    public string Id
    {
      get { return PID; }
    }

    public bool OnOk()
    {
      return true;
    }

    public bool ValidatePage()
    {
      return true;
    }
  }
}
