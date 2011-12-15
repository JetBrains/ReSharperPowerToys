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

using System;
using System.Linq.Expressions;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.Configuration;
using JetBrains.Application.Env;
using JetBrains.Application.Env.Components;
using JetBrains.Application.Settings;
using JetBrains.Application.src.Settings;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model
{
  [ShellComponent(ProgramConfigurations.VS_ADDIN)]
  public class SettingsUpgrader
  {
    public SettingsUpgrader(ISettingsStore settingsStore, IApplicationDescriptor applicationDescriptor, IShellLocks locks, ProductSettingsLocation productSettingsLocation, RunsProducts.ProductConfigurations productConfigurations)
    {
      var boundSettingsStore = settingsStore.BindToContextTransient(ContextRange.ManuallyRestrictWritesToOneContext((l, contexts) => contexts.Empty));
      Expression<Func<ZenCodingSettings, bool>> isUpgradedProperty = settings => settings.IsUpgraded;
      if (!boundSettingsStore.GetValue(isUpgradedProperty))
      {
        var settingsComponent = new ShellSettingsComponent(applicationDescriptor, locks, productSettingsLocation, productConfigurations);
        var oldSettings = new Settings();
        settingsComponent.LoadSettings(oldSettings, XmlExternalizationScope.UserSettings, oldSettings.GetType().Name);
        boundSettingsStore.SetValue((ZenCodingSettings settings) => settings.FileAssociations, oldSettings.FileAssociations);
        boundSettingsStore.SetValue(isUpgradedProperty, true);
      }
    }
  }
}