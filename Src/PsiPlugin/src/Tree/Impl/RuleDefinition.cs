using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  internal partial class RuleDefinition : IDeclaration, IDeclaredElement
  {
    public void SetName(string name)
    {
      PsiTreeUtil.ReplaceRuleName(this, RuleName, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode ruleName = RuleName;
      int offset = ruleName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), ruleName.GetText().Length); //ruleName.GetNavigationRange());
    }

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration>();
      list.Add(this);
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      var list = new List<IDeclaration>();
      list.Add(this);
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
      return false;
    }

    public string ShortName
    {
      get { return RuleName.GetText(); }
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
      get { return getDeclaredName(); }
    }

    private string getDeclaredName()
    {
      return RuleName.GetText();
    }
  }
}
