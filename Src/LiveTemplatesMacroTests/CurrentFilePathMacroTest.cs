using System;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.LiveTemplatesTests.Macros;
using JetBrains.ReSharper.PowerToys.LiveTemplatesMacro;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace LiveTemplatesMacroTests
{
  public class CurrentFilePathMacroTest : MacroTestBase
  {
    protected override IMacro Macro
    {
      get { return new CurrentFilePathMacro(); }
    }

    protected override string RelativeTestDataPath
    {
      get { return @"CurrentFilePathMacro\"; }
    }

    [Test]
    public void Test1()
    {
      DoTestFiles("NoSuchFile");
    }
  }
}
