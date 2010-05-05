using JetBrains.ActionManagement;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  public static class TypeInterfaceUtil
  {
    #region Operations

    /// <summary>
    /// Gets type element from context 
    /// </summary>
    /// <param name="context">Action context</param>
    /// <param name="instance">If true, only instance memebers are relevant, e.g. context points to instance of type</param>
    /// <returns>ITypeElement instance or null</returns>
    public static ITypeElement GetTypeElement(IDataContext context, out bool instance)
    {
      // Obtain declared element from context
      // This may be from source code (from caret), or from various tree views displaying elements
      IDeclaredElement declaredElement = context.GetData(DataConstants.DECLARED_ELEMENT);

      // If we have type, just return it
      var typeElement = declaredElement as ITypeElement;
      if(typeElement != null)
      {
        instance = false;
        return typeElement;
      }

      // If it is constructor, return containing type
      var constructor = declaredElement as IConstructor;
      if(constructor != null)
      {
        instance = false;
        return constructor.GetContainingType();
      }

      // Element has type attached to it, e.g. method return type, property or field type
      var typeOwner = declaredElement as ITypeOwner;
      if(typeOwner != null)
      {
        // It is instance of type, which is returned, so provide caller with this information
        instance = true;
        return GetTypeElement(typeOwner.Type);
      }

      // Try to guess type of expression
      ITextControl textControl = context.GetData(IDE.DataConstants.TEXT_CONTROL);
      ISolution solution = context.GetData(IDE.DataConstants.SOLUTION);
      if(textControl != null && solution != null)
      {
        // TODO: Implement expression processing
      }

      instance = false;
      return null;
    }

    /// <summary>
    /// Gets root type element from composite type like array or pointer
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ITypeElement GetTypeElement(IType type)
    {
      var declaredType = type as IDeclaredType;
      if(declaredType != null)
      {
        // skip System.Void type, we don't care about its members
        if(declaredType.IsVoid())
          return null;
        return declaredType.GetTypeElement();
      }

      // For array or pointer type, get it's element type and process recursively

      var arrayType = type as IArrayType;
      if(arrayType != null)
        return GetTypeElement(arrayType.ElementType);

      var pointerType = type as IPointerType;
      if(pointerType != null)
        return GetTypeElement(pointerType.ElementType);

      return null;
    }

    #endregion
  }
}