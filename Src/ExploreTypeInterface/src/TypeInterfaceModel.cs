/*
 * Copyright 2007-2011 JetBrains s.r.o.
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve.Filters;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.TreeModels;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Model for building tree of exploration results, inherits TreeDemandModel to build tree on node expansion
  /// </summary>
  public class TypeInterfaceModel : TreeDemandModel
  {
    // Root type element envoy

    // Hide static and protected members for root, if true
    private readonly bool myInstanceOnly;
    private readonly DeclaredElementEnvoy<ITypeElement> myTypeElementEnvoy;

    public TypeInterfaceModel(DeclaredElementEnvoy<ITypeElement> typeElementEnvoy, bool instanceOnly)
    {
      myTypeElementEnvoy = typeElementEnvoy;
      myInstanceOnly = instanceOnly;
    }

    // Quickly determine if node has children
    // Can return true and then return no children, but not the opposite
    protected override bool HasChildren(TreeModelNode modelNode)
    {
      // Get value associated with node
      object dataValue = modelNode.DataValue;
      if (dataValue == null)
        return true;

      var envoy = dataValue as IDeclaredElementEnvoy;
      if (envoy != null)
      {
        // Get type which will be expanded for the node
        IDeclaredElement declaredElement = envoy.GetValidDeclaredElement();
        var typeElement = declaredElement as ITypeElement;
        if (typeElement == null)
        {
          var typeOwner = declaredElement as ITypeOwner;
          if (typeOwner != null && !(typeOwner is IEvent))
            typeElement = TypeInterfaceUtil.GetTypeElement(typeOwner.Type);
          else
          {
            var method = declaredElement as IMethod;
            if (method != null)
              typeElement = TypeInterfaceUtil.GetTypeElement(method.ReturnType);
          }
        }
        // Type cannot be determined (or void)
        if (typeElement == null)
          return false;

        // Do we have members for type?
        return !typeElement.GetMembers().IsEmpty();
      }
      return false;
    }

    protected override IEnumerable GetChildren(TreeModelNode modelNode)
    {
      // dataValue is null when requesting root elements, we have only one
      if (modelNode == null)
        return new[] {myTypeElementEnvoy};

      var envoy = modelNode.DataValue as IDeclaredElementEnvoy;
      if (envoy == null)
        return EmptyArray<IDeclaredElementEnvoy>.Instance;

      bool instanceVisible = envoy != myTypeElementEnvoy || myInstanceOnly;
      IDeclaredElement declaredElement = envoy.GetValidDeclaredElement();

      // Get type which is expanded for the node
      var typeElement = declaredElement as ITypeElement;
      if (typeElement == null)
      {
        var typeOwner = declaredElement as ITypeOwner;
        if (typeOwner != null && !(typeOwner is IEvent))
          typeElement = TypeInterfaceUtil.GetTypeElement(typeOwner.Type);
        else
        {
          var method = declaredElement as IMethod;
          if (method != null)
            typeElement = TypeInterfaceUtil.GetTypeElement(method.ReturnType);
        }
      }
      if (typeElement == null)
        return EmptyArray<DeclaredElementEnvoy<ITypeElement>>.Instance;

      return GetChildren(typeElement, instanceVisible);
    }

    private static IEnumerable GetChildren(ITypeElement typeElement, bool instanceVisible)
    {
      // Obtain language service for the type
      PsiLanguageType language = PresentationUtil.GetPresentationLanguage(typeElement);
      if (language.IsNullOrUnknown())
        return Enumerable.Empty<DeclaredElementEnvoy<ITypeMember>>();

      LanguageService languageService = language.LanguageService();
      if (languageService == null)
        return Enumerable.Empty<DeclaredElementEnvoy<ITypeMember>>();

      // Get symbol table for the typeElement and filter it with OverriddenFilter
      // This filter removes all but leaf members for override chains
      ISymbolTable symbolTable = TypeFactory.CreateType(typeElement).GetSymbolTable(typeElement.Module);
      symbolTable = symbolTable.Filter(OverriddenFilter.INSTANCE);

      // Obtain ITypeElement for System.Object 
      // We don't want ToString(), GetHashCode(), GetType() and Equals() to pollute tree view
      ITypeElement objectType = typeElement.Module.GetPredefinedType().Object.GetTypeElement();
      var children = new List<DeclaredElementEnvoy<ITypeMember>>();
      foreach (string name in symbolTable.Names())
        foreach (ISymbolInfo info in symbolTable.GetSymbolInfos(name))
        {
          // Select all ITypeMembers from symbol table
          var member = info.GetDeclaredElement() as ITypeMember;
          if (member == null)
            continue;
          // Checks that member is visible in language. For example, get_Property() member is not visible in C#
          if (!languageService.IsTypeMemberVisible(member))
            continue;
          // Do not show members of System.Object
          // This doesn't hide respective overrides, though
          if (Equals(member.GetContainingType(), objectType))
            continue;
          // Checks that member is not "synthetic". Synthetic members are generated by ReSharper to support 
          // generative languages, like ASP.NET
          if (member.IsSynthetic())
            continue;

          // Checks if member should be displayed according to its accessibility
          AccessibilityDomain.AccessibilityDomainType access = member.AccessibilityDomain.DomainType;
          if (access == AccessibilityDomain.AccessibilityDomainType.PRIVATE)
            continue;

          // If we want only instance members, filter further
          if (instanceVisible)
          {
            // Don't show nested types 
            if (member is ITypeElement)
              continue;
            // Don't show constructors
            if (member is IConstructor)
              continue;

            // hide static, protected and "protected internal" members
            if (member.IsStatic)
              continue;
            if (access == AccessibilityDomain.AccessibilityDomainType.PROTECTED)
              continue;
            if (access == AccessibilityDomain.AccessibilityDomainType.PROTECTED_AND_INTERNAL)
              continue;
          }
          // It passed all filters! Create an envoy and add to collection
          children.Add(new DeclaredElementEnvoy<ITypeMember>(member));
        }

      return children;
    }
  }
}