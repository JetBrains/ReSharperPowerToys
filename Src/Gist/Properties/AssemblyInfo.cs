﻿/*
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

using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;
using JetBrains.UI;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("ReSharper PowerToys: Gist")]
[assembly: AssemblyDescription("With this ReSharper PowerToy you can easily and transparently publish your code snippets on Gist.")]
[assembly: AssemblyCompany("JetBrains")]
[assembly: AssemblyProduct("ReSharper PowerToys")]
[assembly: AssemblyCopyright("Copyright \u00A9 2006-2014 JetBrains All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("8.2")]
[assembly: AssemblyFileVersion("8.2")]
[assembly: ComVisible(false)]
[assembly: Guid("04800884-1E07-41ED-A664-6454C7EA5456")]

[assembly: PluginTitle("ReSharper PowerToys: Gist")]
[assembly: PluginVendor("JetBrains")]
[assembly: PluginDescription("Publish code snippets on Gist (https://gist.github.com/)")]

[assembly: ActionsXml("JetBrains.ReSharper.PowerToys.Gist.resources.actions.xml")]
