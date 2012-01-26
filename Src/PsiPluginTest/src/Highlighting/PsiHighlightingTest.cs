using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Test;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using NUnit.Framework;

namespace PsiPluginTest.Highlighting
{
  public class PsiHighlightingTest : HighlightingTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"highlighting"; }
    }

    public override ProjectLanguage DefaultProjectLanguage
    {
      get { return ProjectLanguage.UNKNOWN; }
    }

    protected override PsiLanguageType CompilerIdsLanguage
    {
      get { return PsiLanguage.Instance; }
    }

    [Test]
    public void test001() { DoTestFiles("test001.psi"); }
    [Test]
    public void test002() { DoTestFiles("test002.psi"); }
  }
}
