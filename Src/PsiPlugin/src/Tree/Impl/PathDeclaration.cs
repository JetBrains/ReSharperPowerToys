using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  partial class PathDeclaration : IDeclaredElement
  {
    public void SetName(string name)
    {
      PsiTreeUtil.ReplaceChild(PathName, PathName.FirstChild, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode pathName = PathName;
      int offset = pathName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), pathName.GetText().Length); //ruleName.GetNavigationRange());
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
      return PsiDeclaredElementType.Path;
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
      get { return PathName.GetText(); }
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
      return PathName.GetText();
    }

    public PsiLanguageType PresentationLanguage
    {
      get { return PsiLanguage.Instance; }
    }
  }
}
