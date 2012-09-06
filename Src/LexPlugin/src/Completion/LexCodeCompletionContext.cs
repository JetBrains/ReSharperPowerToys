using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;

namespace JetBrains.ReSharper.LexPlugin.Completion
{
  class LexCodeCompletionContext : SpecificCodeCompletionContext
  {
    public LexCodeCompletionContext(CodeCompletionContext context, TextLookupRanges completionRanges, LexReparsedCompletionContext reparsedContext)
      :
        base(context)
    {
      ReparsedContext = reparsedContext;
      Ranges = completionRanges;
    }

    public TextLookupRanges Ranges { get; private set; }

    public LexReparsedCompletionContext ReparsedContext { get; private set; }

    public override string ContextId
    {
      get { return "LexSpecificContext"; }
    }
  }
}
