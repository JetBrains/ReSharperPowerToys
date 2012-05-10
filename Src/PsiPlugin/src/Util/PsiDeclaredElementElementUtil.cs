using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Util
{
  internal static class PsiDeclaredElementUtil
  {
    /// <summary>
    ///   Check if the member is visible as C# type member I.e. it skips accessors except to properties with parameters
    /// </summary>
    public static bool IsCollectionInitializerAddMethod(IDeclaredElement declaredElement)
    {
      var method = declaredElement as IMethod;
      if (method == null)
      {
        return false;
      }
      if (method.IsStatic)
      {
        return false;
      }
      if (method.ShortName != "Add")
      {
        return false;
      }

      ITypeElement containingType = method.GetContainingType();
      if (containingType == null)
      {
        return false;
      }

      if (method.Parameters.Any(parameter => parameter.Kind != ParameterKind.VALUE))
      {
        return false;
      }

      if (!containingType.IsDescendantOf(method.Module.GetPredefinedType().IEnumerable.GetTypeElement()))
      {
        return false;
      }

      return true;
    }

    public static bool IsForeachEnumeratorPatternMember(IDeclaredElement declaredElement)
    {
      var method = declaredElement as IMethod;
      if (method != null)
      {
        // GetEnumerator
        if (method.ShortName == "GetEnumerator" &&
          method.Parameters.Count == 0 &&
            !method.IsStatic &&
              method.TypeParameters.Count == 0 &&
                !method.IsExplicitImplementation &&
                  method.GetAccessRights() == AccessRights.PUBLIC)
        {
          return true;
        }

        if (IsForeachMoveNextMethodCandidate(method))
        {
          ITypeElement containingType = method.GetContainingType();
          if (containingType != null)
          {
            ISymbolTable symbolTable = ResolveUtil.GetSymbolTableByTypeElement(containingType, SymbolTableMode.FULL,
              containingType.Module);
            if (
              symbolTable.GetSymbolInfos("Current").Any(
                symbolInfo => IsForeachCurrentPropertyCandidate(symbolInfo.GetDeclaredElement() as IProperty)))
            {
              return true;
            }
          }
        }
      }

      var property = declaredElement as IProperty;
      if (property != null)
      {
        if (IsForeachCurrentPropertyCandidate(property))
        {
          ITypeElement containingType = property.GetContainingType();
          if (containingType != null)
          {
            ISymbolTable symbolTable = ResolveUtil.GetSymbolTableByTypeElement(containingType, SymbolTableMode.FULL,
              containingType.Module);
            if (
              symbolTable.GetSymbolInfos("MoveNext").Any(
                symbolInfo => IsForeachMoveNextMethodCandidate(symbolInfo.GetDeclaredElement() as IMethod)))
            {
              return true;
            }
          }
        }
      }

      return false;
    }

    private static bool IsForeachMoveNextMethodCandidate(IMethod method)
    {
      return method != null &&
        method.ShortName == "MoveNext" &&
          method.Parameters.Count == 0 &&
            !method.IsExplicitImplementation &&
              method.GetAccessRights() == AccessRights.PUBLIC &&
                !method.IsStatic &&
                  method.ReturnType.IsBool();
    }

    private static bool IsForeachCurrentPropertyCandidate(IProperty property)
    {
      if (property != null &&
        property.ShortName == "Current" &&
          property.Parameters.Count == 0 &&
            property.GetAccessRights() == AccessRights.PUBLIC &&
              !property.IsStatic)
      {
        IAccessor getter = property.GetPolymorhicGetter();
        if (getter != null && getter.GetAccessRights() == AccessRights.PUBLIC)
        {
          return true;
        }
      }
      return false;
    }
  }
}
