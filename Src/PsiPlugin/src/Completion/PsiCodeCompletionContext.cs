using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  public class PsiCodeCompletionContext : SpecificCodeCompletionContext
  {
    public TextLookupRanges Ranges { get; private set; }

    public PsiReparsedCompletionContext ReparsedContext { get; private set; }

    public PsiCodeCompletionContext(CodeCompletionContext context, TextLookupRanges completionRanges, PsiReparsedCompletionContext reparsedContext) : 
      base(context)
    {
      ReparsedContext = reparsedContext;
      Ranges = completionRanges;
    }

    public override string ContextId
    {
      get
      {
        return "PsiSpecificContext";
      }
    }
  }
}
