using System.Xml;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Jam.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal partial class LocalVariableDeclarationStatement
  {
    [CanBeNull] private IDeclaredElement myDeclaredElement;

    public string DeclaredName
    {
      get
      {
        if (Name == null)
          return SharedImplUtil.MISSING_DECLARATION_NAME;

        return Name.Name;
      }
    }

    public IDeclaredElement DeclaredElement
    {
      get
      {
        Assertion.Assert(IsValid(), "Asking declared element from invalid declaration");
        return myDeclaredElement ?? (myDeclaredElement = new JamLocalVariableDeclaredElement(this));
      }
    }

    ILocalVariableDeclaredElement ILocalVariableDeclaration.DeclaredElement
    {
      get { return (ILocalVariableDeclaredElement) DeclaredElement; }
    }

    public void SetName(string name)
    {
      var newName = JamElementFactory.GetInstance(parent.GetPsiModule()).CreateExpression<IIdentifierExpression>(name);
      SetName(newName.LiteralToken);
    }

    public TreeTextRange GetNameRange()
    {
      return Name != null ? Name.GetTreeTextRange() : TreeTextRange.InvalidRange;
    }

    public bool IsSynthetic()
    {
      return false;
    }

    public XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }
  }
}