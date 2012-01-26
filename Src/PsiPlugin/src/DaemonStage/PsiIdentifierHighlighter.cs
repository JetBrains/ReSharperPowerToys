using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.DaemonStage
{
  public class PsiIdentifierHighlighter
  {
  private readonly IReferenceProvider myReferenceProvider;

    public PsiIdentifierHighlighter([NotNull] ITreeNode root)
    {
      Assertion.Assert(root != null, "root != null");
      myReferenceProvider = ((IFileImpl)root.GetContainingFile()).ReferenceProvider;
    }

    public void Highlight (ITreeNode element, Action<DocumentRange, IHighlighting> consumer)
    {
      if (element is IDocCommentBlockNode)
        return;

      var declaration = element as IDeclaration;
      if (declaration != null)
      {
        var highlighting = Highlight(declaration.DeclaredElement);
        if (highlighting != null)
          consumer(declaration.GetNameDocumentRange(), highlighting);
        return;
      }

      //if ((element is IAnonymousMemberDeclaration) && ((IAnonymousMemberDeclaration)element).IsProjectionInitializer) 
        //return;
      //if (element is IAwaitExpression)
        //return;

      foreach (IReference reference in element.GetReferences(null, myReferenceProvider))
      {
        // skip some cases
        /*var creationExpression = element as IObjectCreationExpression;
        if (creationExpression != null && reference == creationExpression.ConstructorReference) continue;

        if (element is ICollectionElementInitializer) continue;

        var attribute = element as IAttribute;
        if (attribute != null && reference == attribute.ConstructorReference) continue;

        if (reference is IForeachStatementReference) continue;
        if (reference is IElementAccessExpressionReference) continue;
        if (reference is IPredefinedTypeReference) continue;
        if (reference is IQueryReference) continue;*/

        //
        IResolveResult result = reference.Resolve().Result;

        /*if (result.IsLateBound())
        {
          consumer(reference.GetDocumentRange(), new PsiIdentifierHighlighting(HighlightingAttributeIds.LATE_BOUND_IDENTIFIER_ATTRIBUTE));
          continue;
        }*/

        if (result.DeclaredElement != null)
        {
          var highlighting = Highlight(result.DeclaredElement);
          if (highlighting != null)
            consumer(reference.GetDocumentRange(), highlighting);
        }
        else
        {
          IList<IDeclaredElement> candidates = result.Candidates;
          if (candidates.Count > 0)
          {
            DeclaredElementType elementType = candidates[0].GetElementType();
            if (candidates.All((candidate => candidate.GetElementType() == elementType)))
            {
              var highlighting = Highlight(candidates[0]);
              if (highlighting != null)
                consumer(reference.GetDocumentRange(), highlighting);
            }
          }
        }
      }
    }

    private static IHighlighting Highlight(IDeclaredElement declaredElement)
    {
      string attribute = GetHighlightAttribute(declaredElement);
      return attribute != null ? new IdentifierPsiHighlighting(attribute) : null;
    }

    private static string GetHighlightAttribute(IDeclaredElement declaredElement)
    {
      if (declaredElement is IFunction)
      {
        var method = declaredElement as IMethod;
        if (method != null)
        {
          if (method.IsPredefined) return null;
          if (method.IsExtensionMethod) return HighlightingAttributeIds.EXTENSION_METHOD_IDENTIFIER_ATTRIBUTE;
          return HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE;
        }

        var @operator = declaredElement as ISignOperator;
        if (@operator != null)
          return @operator.IsPredefined ? null : HighlightingAttributeIds.OPERATOR_IDENTIFIER_ATTRIBUTE;

        if (declaredElement is IConstructor)
        {
          var typeElement = ((IConstructor)declaredElement).GetContainingType();
          if (typeElement == null)
            return null;
          return HighlightingAttributeIds.GetHighlightAttributeForTypeElement(typeElement);
        }
      }

      var field = declaredElement as IField;
      if (field != null)
      {
        return field.IsField
                 ? HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE
                 : HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE;
      }

      if (declaredElement is ITypeElement)
        return HighlightingAttributeIds.GetHighlightAttributeForTypeElement((ITypeElement) declaredElement);

      //if (PsiDeclaredElementUtil.IsProperty(declaredElement) || PsiDeclaredElementUtil.IsIndexedProperty(declaredElement))
        //return HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE;

      if (declaredElement is IEvent)
        return HighlightingAttributeIds.EVENT_IDENTIFIER_ATTRIBUTE;

      if (declaredElement is INamespace)
        return HighlightingAttributeIds.NAMESPACE_IDENTIFIER_ATTRIBUTE;

      if (declaredElement is IParameter)
        return (((IParameter)declaredElement).IsValueVariable) ? null : HighlightingAttributeIds.PARAMETER_IDENTIFIER_ATTRIBUTE;

      if (declaredElement is ILocalVariable)
        return ((ILocalVariable)declaredElement).IsConstant
                 ? HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE
                 : HighlightingAttributeIds.LOCAL_VARIABLE_IDENTIFIER_ATTRIBUTE;

      /*if (declaredElement is IPsiAnonymousTypeProperty)
      {
        if (declaredElement is IQueryAnonymousTypeProperty)
          return HighlightingAttributeIds.LOCAL_VARIABLE_IDENTIFIER_ATTRIBUTE;
        return HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE;
      }*/

      return null;
    }
  }
}
