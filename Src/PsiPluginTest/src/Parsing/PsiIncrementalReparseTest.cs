﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace PsiPluginTest.Parsing
{
  [Category("PSI")]
  [TestFileExtension(PsiProjectFileType.PSI_EXTENSION)]
  class PsiIncrementalReparseTest : IncrementalReparseTestBase
  {
    protected override String RelativeTestDataPath
    {
      get { return @"parsing\reparse"; }
    }

    [Test]
    public void test001()
    {
      //DoNamedTest();
      DoTestFiles("test001.psi");
    }
  }
}