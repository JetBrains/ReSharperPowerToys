using System;
using System.Collections.Generic;
using JetBrains.ProjectModel.Properties;
using JetBrains.ProjectModel.Properties.Common;
using JetBrains.ReSharper.Daemon.Test;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using NUnit.Framework;
using PlatformID = JetBrains.ProjectModel.PlatformID;

namespace PsiPluginTest.Highlighting
{
  public class PsiHighlightingTest : HighlightingTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"highlighting"; }
    }

    public override IProjectProperties GetProjectProperties(PlatformID platformId, ICollection<IProjectFlavour> flavours)
    {
      return UnknownProjectPropertiesFactory.CreateUnknownProjectProperties(platformId);
    }

    protected override PsiLanguageType CompilerIdsLanguage
    {
      get { return PsiLanguage.Instance; }
    }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.psi");
    }
    [Test]
    public void test002()
    {
      DoTestFiles("test002.psi");
    }
    [Test]
    public void test003()
    {
      DoTestFiles("test003.psi");
    }
  }
}
