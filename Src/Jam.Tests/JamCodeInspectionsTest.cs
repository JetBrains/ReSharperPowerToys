using System;
using System.Collections.Generic;
using JetBrains.ProjectModel.Properties;
using JetBrains.ProjectModel.Properties.CSharp;
using JetBrains.ReSharper.Daemon.Test;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using PlatformID = JetBrains.ProjectModel.PlatformID;

namespace JetBrains.ReSharper.Psi.Jam.Tests
{
  [TestFixture]
  [Category("Daemon")]
  [TestFileExtension(".jam")]
  public class JamCodeInspectionsTest : HighlightingTestBase
  {
    protected override string RelativeTestDataPath
    {
      get { return @"codeInspections"; }
    }

    protected override PsiLanguageType CompilerIdsLanguage
    {
      get { return JamLanguage.Instance; }
    }

    public override IProjectProperties GetProjectProperties(PlatformID platformId, ICollection<Guid> flavours)
    {
      return CSharpProjectPropertiesFactory.CreateCSharpProjectProperties(platformId);
    }

    [Test] public void Test001() {DoNamedTest();}
    [Test] public void Test002() {DoNamedTest();}
  }
}