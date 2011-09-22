using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Subclasses default presenter to avoid member qualification
  /// </summary>
  internal class TypeInterfacePresenter : TreeModelBrowserPresenter
  {
    protected override bool IsNaturalParent(object parentValue, object childValue)
    {
      // Never qualify members which are not types
      if (childValue is ITypeMember && !(childValue is ITypeElement))
        return true;
      return base.IsNaturalParent(parentValue, childValue);
    }
  }
}