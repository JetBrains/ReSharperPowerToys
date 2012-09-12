using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.Psi.Jam.Resolve
{
  public static class ResolveInfoEx
  {
    public static IResolveInfo CheckResolveInfo(this IResolveInfo result, ResolveErrorType notResolved)
    {
      return Equals(result, ResolveErrorType.NOT_RESOLVED) ? notResolved : result;
    }
  }
}