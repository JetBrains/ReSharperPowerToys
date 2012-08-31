using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl
{
  internal partial class PathDeclaration : IDeclaredElement
  {
    #region IDeclaredElement Members

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration> { this };
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      var list = new List<IDeclaration> { this };
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

    public PsiLanguageType PresentationLanguage
    {
      get { return PsiLanguage.Instance; }
    }

    #endregion

    #region IPathDeclaration Members

    public void SetName(string name)
    {
      PsiTreeUtil.ReplaceChild(PathName, PathName.FirstChild, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode pathName = PathName;
      int offset = pathName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), pathName.GetText().Length);
    }

    public IDeclaredElement DeclaredElement
    {
      get { return this; }
    }

    public string DeclaredName
    {
      get { return GetDeclaredName(); }
    }

    #endregion

    private string GetDeclaredName()
    {
      return PathName.GetText();
    }
  }
}
