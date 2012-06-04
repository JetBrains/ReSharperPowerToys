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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.CSharp.Generate;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Util;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
  [GeneratorBuilder("Dispose", typeof(CSharpLanguage))]
  internal class CSharpDisposeBuilder : GeneratorBuilderBase<CSharpGeneratorContext>
  {
    private readonly CodeAnnotationsCache myCodeAnnotationsCache;

    public CSharpDisposeBuilder(CodeAnnotationsCache codeAnnotationsCache)
    {
      myCodeAnnotationsCache = codeAnnotationsCache;
    }

    public override double Priority
    {
      get { return 0; }
    }

    protected override void Process(CSharpGeneratorContext context)
    {
      if (context.ClassDeclaration == null)
        return;

      var factory = CSharpElementFactory.GetInstance(context.Root.GetPsiModule());
      var typeOwners = context.InputElements.OfType<GeneratorDeclaredElement<ITypeOwner>>().ToList();

      // order is important
      CreateDispose(context, factory, typeOwners);
      if (context.GetGlobalOptionValue("ImplementDisposable") == bool.TrueString)
      {
        var disposableInterface = GetDisposableInterface(context);
        if (disposableInterface != null)
        {
          var ownTypeElement = context.ClassDeclaration.DeclaredElement;
          if (ownTypeElement != null)
            context.ClassDeclaration.AddSuperInterface(TypeFactory.CreateType(disposableInterface), false);
        }
      }
    }

    private static void CreateDispose(CSharpGeneratorContext context, CSharpElementFactory factory,
                                      ICollection<GeneratorDeclaredElement<ITypeOwner>> typeOwners)
    {
      var existingEquals = FindDispose(context);
      IMethodDeclaration declaration;
      if (existingEquals != null)
      {
        if (context.GetGlobalOptionValue("ChangeDispose") == "Skip")
          return;
        if (context.GetGlobalOptionValue("ChangeDispose") == "Replace")
        {
          declaration = (IMethodDeclaration)existingEquals.GetDeclarations().FirstOrDefault();
          GenerateDisposeBody(context, declaration, typeOwners, factory);
          return;
        }
      }
      declaration = (IMethodDeclaration)factory.CreateTypeMemberDeclaration(
                                          "public void Dispose();");
      GenerateDisposeBody(context, declaration, typeOwners, factory);
      context.PutMemberDeclaration(declaration, null, newDeclaration => new GeneratorDeclarationElement(newDeclaration));
    }

    private static void GenerateDisposeBody(CSharpGeneratorContext context, ICSharpFunctionDeclaration methodDeclaration, ICollection<GeneratorDeclaredElement<ITypeOwner>> elements, CSharpElementFactory factory)
    {
      var builder = new StringBuilder();
      var owner = (IParametersOwner)methodDeclaration.DeclaredElement;
      if (owner == null)
        return;

      var typeElement = context.ClassDeclaration.DeclaredElement;
      var args = new List<object>();

      if (elements.Count == 0)
      {
        if (typeElement is IClass)
          builder.Append("base.Dispose();");
        methodDeclaration.SetBody(factory.CreateBlock("{" + builder + "}", args.ToArray()));
        return;
      }

      foreach (var element in elements)
      {
        var typeOwner = element.DeclaredElement;
        var type = typeOwner.Type;
        if (type.IsReferenceType() && context.GetElementOptionValue(element, CSharpBuilderOptions.CanBeNull) == bool.TrueString)
          builder.Append("if ($" + args.Count + " != null) $" + args.Count + ".Dispose();");
        else
          builder.Append("$" + args.Count + ".Dispose();");
        args.Add(typeOwner);
      }
      methodDeclaration.SetBody(factory.CreateBlock("{" + builder + "}", args.ToArray()));
    }


    protected override IList<IGeneratorOption> GetGlobalOptions(CSharpGeneratorContext context)
    {
      var hasReferenceFields = context.ProvidedElements
        .OfType<GeneratorDeclaredElement<ITypeOwner>>()
        .Any(field => field.DeclaredElement.Type.IsReferenceType());

      var options = new List<IGeneratorOption>();
      if (hasReferenceFields)
        options.Add(new GeneratorOptionBoolean(CSharpBuilderOptions.CanBeNull, "Fields can be &null", true));
      if (FindDispose(context) != null)
        options.Add(new GeneratorOptionSelector("ChangeDispose", "&Dispose already exists", "Replace", new[] { "Replace", "Skip", "Side by side" }) { Persist = true });
      if (!HasDisposable(context))
        options.Add(new GeneratorOptionBoolean("ImplementDisposable", "I&mplement IDisposable interface", true) { Persist = true });
      return options;
    }

    private static bool HasDisposable(CSharpGeneratorContext context)
    {
      var disposableInterface = GetDisposableInterface(context);
      if (disposableInterface == null)
        return false;
      var ownTypeElement = context.ClassDeclaration.DeclaredElement;
      if (ownTypeElement == null)
        return false;
      var ownType = TypeFactory.CreateType(ownTypeElement);
      var disposableType = TypeFactory.CreateType(disposableInterface);
      return ownType.IsSubtypeOf(disposableType);
    }

    private static ITypeElement GetDisposableInterface(IGeneratorContext context)
    {
      return TypeFactory.CreateTypeByCLRName("System.IDisposable", context.PsiModule).GetTypeElement();
    }

    private static IOverridableMember FindDispose(CSharpGeneratorContext context)
    {
      if (context.ClassDeclaration.DeclaredElement == null)
        return null;

      return context.ClassDeclaration.DeclaredElement.Methods
        .FirstOrDefault(method => method.ShortName == "Dispose"
                                  && method.ReturnType.IsVoid()
                                  && method.Parameters.Count == 0);
    }

    protected override IList<IGeneratorOption> GetInputElementOptions(IGeneratorElement inputElement,
                                                                      CSharpGeneratorContext context)
    {
      var declaredElement = inputElement as GeneratorDeclaredElement<ITypeOwner>;
      if (declaredElement != null)
      {
        var typeOwner = declaredElement.DeclaredElement;
        if (typeOwner.Type.IsReferenceType())
        {
          var attributesOwner = typeOwner as IAttributesOwner;
          var mark = myCodeAnnotationsCache.GetNullableAttribute(attributesOwner);
          return new IGeneratorOption[]
          {
            new GeneratorOptionBoolean(CSharpBuilderOptions.CanBeNull, "Can be &null",
                                       mark != CodeAnnotationNullableValue.NOT_NULL)
              { OverridesGlobalOption = mark == CodeAnnotationNullableValue.NOT_NULL }
          };
        }
      }
      return base.GetInputElementOptions(inputElement, context);
    }

    protected override bool HasProcessableElements(CSharpGeneratorContext context, IEnumerable<IGeneratorElement> elements)
    {
      return true;
    }
  }
}