using System;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Sorts by member kind first, then by standard member order - name, visibility, genericity, etc
  /// </summary>
  internal class TypeInterfaceModelComparer : TreeModelBrowserComparer
  {
    protected override int CompareTypeMember(ITypeMember x, ITypeMember y)
    {
      DeclaredElementType xType = x.GetElementType();
      DeclaredElementType yType = y.GetElementType();
      if (xType.Equals(yType))
        return base.CompareTypeMember(x, y);

      return StringComparer.InvariantCultureIgnoreCase.Compare(xType.PresentableName, yType.PresentableName);
    }
  }
}