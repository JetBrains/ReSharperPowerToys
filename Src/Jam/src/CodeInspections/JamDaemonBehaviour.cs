using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections
{
  [Language(typeof (JamLanguage))]
  internal class JamDaemonBehaviour : ILanguageSpecificDaemonBehavior
  {
    public ErrorStripeRequest InitialErrorStripe(IPsiSourceFile sourceFile)
    {
      return ErrorStripeRequest.STRIPE_AND_ERRORS;
    }

    public bool CanShowErrorBox
    {
      get { return true; }
    }

    public bool RunInSolutionAnalysis
    {
      get { return true; }
    }
  }
}
