using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class VariableDeclaration : IDeclaredElement
  {
    public void SetName(string name)
    {
      PsiTreeUtil.ReplaceChild(FirstChild, FirstChild.FirstChild, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode variableName = FirstChild;
      int offset = variableName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), variableName.GetText().Length);
    }

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration> {this};
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      var list = new List<IDeclaration> {this};
      return list;
    }

    public DeclaredElementType GetElementType()
    {
      return PsiDeclaredElementType.Variable;
    }

    public XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    public XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      return null;
    }

    public bool IsSynthetic()
    {
      return false;
    }

    public string ShortName
    {
      get { return GetText(); }
    }

    public bool CaseSensistiveName
    {
      get { return true; }
    }

    public IDeclaredElement DeclaredElement
    {
      get { return this; }
    }

    public string DeclaredName
    {
      get { return GetDeclaredName(); }
    }

    private string GetDeclaredName()
    {
      return GetText();
    }

    public PsiLanguageType PresentationLanguage
    {
      get { return PsiLanguage.Instance; }
    }
  }
}
