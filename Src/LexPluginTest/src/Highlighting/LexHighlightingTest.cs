using System;
using System.Collections.Generic;
using JetBrains.ProjectModel.Properties;
using JetBrains.ProjectModel.Properties.Common;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.LexPlugin.Grammar;
using NUnit.Framework;
using PlatformID = JetBrains.ProjectModel.PlatformID;

namespace LexPluginTest.Highlighting
{
  public class PsiHighlightingTest : HighlightingTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"highlighting"; }
    }

    public override IProjectProperties GetProjectProperties(PlatformID platformId, ICollection<Guid> flavours)
    {
      return UnknownProjectPropertiesFactory.CreateUnknownProjectProperties(platformId);
    }

    protected override PsiLanguageType CompilerIdsLanguage
    {
      get { return LexLanguage.Instance; }
    }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.lex");
    }
  }
}
