using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.UI.Application.PluginSupport;

[assembly: AssemblyTitle("ReSharper PowerToys: Sample Menu Items")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("Visual Studio 2008")]
[assembly: AssemblyCompany("JetBrains s.r.o.")]
[assembly: AssemblyProduct("ReSharper PowerToys")]
[assembly: AssemblyCopyright("Copyright \u00A9 2006-2008 JetBrains s.r.o. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]
[assembly: PluginTitle("ReSharper PowerToys: Sample Menu Items")]
[assembly: PluginVendor("JetBrains s.r.o.")]
[assembly: PluginDescription("This plugin shows how to add menu items, enable/disable actions depending on context and how to access context information when action is invoked.")]

// references XML-resource with actions configuration for this plug-in

[assembly: ActionsXml("JetBrains.ReSharper.PowerToys.MenuItem.Actions.xml")]
