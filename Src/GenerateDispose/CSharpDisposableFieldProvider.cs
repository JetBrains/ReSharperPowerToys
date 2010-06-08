using System.Linq;
using JetBrains.ReSharper.Feature.Services.CSharp.Generate;
using JetBrains.ReSharper.Feature.Services.Generate;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.GenerateDispose
{
  /// <summary>
  /// Provides input elements for user's choice
  /// </summary>
  [GeneratorElementProvider("Dispose", CSharpLanguageService.CSHARP_LANGUAGEID)]
  internal class CSharpDisposableFieldProvider : CSharpGeneratorProviderBase
  {
    #region IGeneratorElementProvider Members

    /// <summary>
    /// If we have several providers for the same generate kind, this property will set order on them
    /// </summary>
    public override double Priority
    {
      get { return 0; }
    }

    private static ITypeElement GetDisposableInterface(IGeneratorContext context)
    {
      // retrieve ITypeElement for System.IDisposable interface, visible in current project
      return TypeFactory.CreateTypeByCLRName("System.IDisposable", context.PsiModule).GetTypeElement();
    }

    /// <summary>
    /// Populate context with input elements
    /// </summary>
    /// <param name="context"></param>
    public override void Populate(CSharpGeneratorContext context)
    {
      var typeElement = context.ClassDeclaration.DeclaredElement;
      if (typeElement == null)
        return;

      if (!(typeElement is IStruct) && !(typeElement is IClass))
        return;
      var disposableType = TypeFactory.CreateType(GetDisposableInterface(context));

      // We provide elements which are non-static fields, visible to code and implementing IDisposable
      context.ProvidedElements.AddRange(from member in typeElement.GetMembers().OfType<IField>()
                                        let memberType = member.Type as IDeclaredType
                                        where !member.IsStatic
                                              && !member.IsConstant && !member.IsSynthetic()
                                              && memberType != null
                                              && memberType.CanUseExplicitly(context.ClassDeclaration.ToTreeNode())
                                              && memberType.IsSubtypeOf(disposableType)
                                        select new GeneratorDeclaredElement<ITypeOwner>(member));

    }

    #endregion
  }
}