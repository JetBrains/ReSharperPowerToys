/*
 * Copyright 2007-2011 JetBrains
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using DataConstants = JetBrains.ReSharper.Psi.Services.DataConstants;
using JetBrains.Util;
using System.Linq;

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

      var declaredElements = context.GetData(DataConstants.DECLARED_ELEMENTS);
      if (declaredElements == null || declaredElements.IsEmpty())
      {
        instance = false;
        return null;
      }

      IDeclaredElement declaredElement = declaredElements.First();

      // If we have type, just return it
      var typeElement = declaredElement as ITypeElement;
      if (typeElement != null)
      {
        instance = false;
        return typeElement;
      }

      // If it is constructor, return containing type
      var constructor = declaredElement as IConstructor;
      if (constructor != null)
      {
        instance = false;
        return constructor.GetContainingType();
      }

      // Element has type attached to it, e.g. method return type, property or field type
      var typeOwner = declaredElement as ITypeOwner;
      if (typeOwner != null)
      {
        // It is instance of type, which is returned, so provide caller with this information
        instance = true;
        return GetTypeElement(typeOwner.Type);
      }

      // Try to guess type of expression
      ITextControl textControl = context.GetData(TextControl.DataContext.DataConstants.TEXT_CONTROL);
      ISolution solution = context.GetData(ProjectModel.DataContext.DataConstants.SOLUTION);
      if (textControl != null && solution != null)
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
      if (declaredType != null)
      {
        // skip System.Void type, we don't care about its members
        if (declaredType.IsVoid())
          return null;
        return declaredType.GetTypeElement();
      }

      // For array or pointer type, get it's element type and process recursively

      var arrayType = type as IArrayType;
      if (arrayType != null)
        return GetTypeElement(arrayType.ElementType);

      var pointerType = type as IPointerType;
      if (pointerType != null)
        return GetTypeElement(pointerType.ElementType);

      return null;
    }

    #endregion
  }
}