using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Resolve;
using JetBrains.ReSharper.LexPlugin.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl
{
  internal partial class StateDeclaration : IDeclaredElement
  {
    #region Implementation of IDeclaration

    public void SetName(string name)
    {
      LexTreeUtil.ReplaceChild(StateName, StateName.FirstChild, name);
    }

    public TreeTextRange GetNameRange()
    {
      ITreeNode tokenName = StateName;
      int offset = tokenName.GetNavigationRange().TextRange.StartOffset;
      return new TreeTextRange(new TreeOffset(offset), tokenName.GetText().Length);
    }

    public XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    public bool IsSynthetic()
    {
     return false;
    }

    public IList<IDeclaration> GetDeclarations()
    {
      var list = new List<IDeclaration> { this };
      return list;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      if(GetSourceFile() == sourceFile)
      {
        var list = new List<IDeclaration> { this };
        return list;
      }
      return EmptyList<IDeclaration>.InstanceList;
    }

    public DeclaredElementType GetElementType()
    {
      return LexDeclaredElementType.State;
    }


    public XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      return null;
    }

    public string ShortName { get { return StateName.GetText(); } }
    public bool CaseSensistiveName { get { return true; } }
    public PsiLanguageType PresentationLanguage { get { return LexLanguage.Instance; } }


    public IDeclaredElement DeclaredElement
    {
      get { return this; }
    }
    public string DeclaredName
    { 
      get { return StateName.GetText(); } 
    }

    #endregion
  }
}
