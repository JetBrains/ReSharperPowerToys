using JetBrains.ReSharper.Refactorings.Conflicts;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl
{
  public class MakeGenericHierarchyConflictTextProvider : HierarchyConflictTextProviderBase
  {
    public override string WillAlsoOverride()
    {
      return "Converted {0} also overrides {1} that will not be gererified. Please resolve conflict manually.";
    }

    public override string WillAlsoImplement()
    {
      return "Converted {0} also implements {1} that will not be gererified. Please resolve conflict manually.";
    }

    public override string QuasiImplements()
    {
      return "Converted {0} is quasi implemented by {1} that will not be gererified. Please resolve conflict manually.";
    }
  }
}