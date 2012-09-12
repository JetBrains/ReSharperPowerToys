using System;
using System.Xml;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Jam.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  public abstract class JamDeclarationBase : JamCompositeElement, IDeclaration
  {
    [CanBeNull] private IDeclaredElement myDeclaredElement;

    public virtual IDeclaredElement DeclaredElement
    {
      get
      {
        Assertion.Assert(IsValid(), "Asking declared element from invalid declaration");
        return myDeclaredElement ?? (myDeclaredElement = CreateDeclaredElement(this));
      }
    }

    public virtual string DeclaredName
    {
      get
      {
        if (Identifier == null)
          return SharedImplUtil.MISSING_DECLARATION_NAME;

        return Identifier.Name;
      }
    }

    public virtual TreeTextRange GetNameRange()
    {
      return Identifier != null ? Identifier.GetTreeTextRange() : TreeTextRange.InvalidRange;
    }

    public virtual bool IsSynthetic()
    {
      return false;
    }

    public virtual XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    public virtual void SetName(string name)
    {
      try
      {
        var newName = JamElementFactory.GetInstance(parent.GetPsiModule()).CreateExpression<Jam.Tree.IIdentifierExpression>(name);
        SetNameImpl(newName.LiteralToken);
      }
      catch (Exception e)
      {
        Logger.LogExceptionSilently(e);
      }
    }

    protected abstract IJamIdentifier Identifier { get; }

    protected abstract void SetNameImpl([NotNull] IJamIdentifier identifier);

    [NotNull]
    protected abstract IDeclaredElement CreateDeclaredElement(IDeclaration declaration);
  }
}