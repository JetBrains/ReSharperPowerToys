using System.Collections.Generic;
using JetBrains.ReSharper.IntentionsTests.Navigation;
using NUnit.Framework;

namespace LexPluginTest.Navigation
{
  internal class LexAllNavigationProvidersTest : AllNavigationProvidersTestBase
  {
    protected override string ExtraPath
    {
      get { return "..\\..\\navigation"; }
    }

    protected override List<WhatToDump> DumpOptions
    {
      get
      {
        return new List<WhatToDump>(new[]
          {
            WhatToDump.MenuItemPresentation,
            WhatToDump.NavigationResult,
            WhatToDump.OccurenceDump,
            WhatToDump.NavigationProvider,
            WhatToDump.OccurenceType,
            WhatToDump.OccurenceKind
          });
      }
    }

    [Test]
    public void test001()
    {
      DoTestFiles("test001.lex");
    }

    [Test]
    public void test002()
    {
      DoTestFiles("test002.lex");
    }
  }
}