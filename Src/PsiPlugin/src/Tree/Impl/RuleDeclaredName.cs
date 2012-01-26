using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
 internal partial class RuleDeclaredName
  {
    public IList<IDeclaration> GetDeclarations()
    {
      ITreeNode declaredElement = (ITreeNode)getContainingFile().getDeclaration(GetText());
      IList<IDeclaration> list = new List<IDeclaration>();
      list.Add((IDeclaration)declaredElement.Parent);
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      //TODO!!!
      ITreeNode declaredElement = (ITreeNode)getContainingFile().getDeclaration(GetText());
      IList<IDeclaration> list = new List<IDeclaration>();
      list.Add((IDeclaration)declaredElement.Parent);
      return list;
    }

    public DeclaredElementType GetElementType()
    {
      return PsiDeclaredElementType.Rule;
    }

    public XmlNode GetXMLDoc(bool inherit)
    {
      throw new NotImplementedException();
    }

    public XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      throw new NotImplementedException();
    }

    public bool IsSynthetic()
    {
      throw new NotImplementedException();
    }

    public string ShortName
    {
      get { return GetText(); }
    }

    public bool CaseSensistiveName
    {
      get { throw new NotImplementedException(); }
    }

    public override IPsiServices GetPsiServices()
    {
      return getContainingFile().GetPsiServices();
    }
  }
}
