using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  public class JamResolveErrorType : ResolveErrorType 
  {
    public static readonly JamResolveErrorType VARIABLE_NOT_RESOLVED = new JamResolveErrorType("VARIABLE_NOT_RESOLVED");
    public static readonly JamResolveErrorType PROCEDURE_NOT_RESOLVED = new JamResolveErrorType("PROCEDURE_NOT_RESOLVED");

    private JamResolveErrorType(string name) : base(name) {}
  }
}