using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl
{
  public class MethodInvocation
  {
    public IReference Reference { get; private set; }
    public IType Type { get; private set; }
    public IMethod Method { get; private set; }
    public ISubstitution Substitution { get; private set; }

    public MethodInvocation(IReference reference, IType type, IMethod method, ISubstitution substitution)
    {
      Reference = reference;
      Type = type;
      Method = method;
      Substitution = substitution;
    }

    public bool IsValid()
    {
      return Reference.IsValid() && Type.IsValid() && Method.IsValid() && Substitution.IsValid();
    }
  }
}