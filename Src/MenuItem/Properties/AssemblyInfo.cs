/*
 * Copyright 2007-2011 JetBrains
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

using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

[assembly: AssemblyTitle("ReSharper PowerToys: Sample Menu Items")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCompany("JetBrains")]
[assembly: AssemblyProduct("ReSharper PowerToys")]
[assembly: AssemblyCopyright("Copyright \u00A9 2006-2011 JetBrains All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]
[assembly: PluginTitle("ReSharper PowerToys: Sample Menu Items")]
[assembly: PluginVendor("JetBrains")]
[assembly: PluginDescription("This plugin shows how to add menu items, enable/disable actions depending on context and how to access context information when action is invoked.")]

// references XML-resource with actions configuration for this plug-in
[assembly: ActionsXml("JetBrains.ReSharper.PowerToys.MenuItem.resources.actions.xml")]
