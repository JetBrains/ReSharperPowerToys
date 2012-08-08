using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  internal class UnresolvedRuleInterfacesDeclaredElement : IDeclaredElement
  {
    private readonly IPsiSourceFile myFile;
    private readonly string myName;
    private readonly IPsiServices myServices;

    public UnresolvedRuleInterfacesDeclaredElement(IPsiSourceFile file, string name, IPsiServices services)
    {
      myFile = file;
      myName = name;
      myServices = services;
    }

    #region IDeclaredElement Members

    public IPsiServices GetPsiServices()
    {
      return myServices;
    }

    public IList<IDeclaration> GetDeclarations()
    {
      return EmptyList<IDeclaration>.InstanceList;
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return EmptyList<IDeclaration>.InstanceList;
    }

    public DeclaredElementType GetElementType()
    {
      return PsiDeclaredElementType.Option;
    }

    public XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    public XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      return null;
    }

    public bool IsValid()
    {
      return true;
    }

    public bool IsSynthetic()
    {
      return false;
    }

    public HybridCollection<IPsiSourceFile> GetSourceFiles()
    {
      return new HybridCollection<IPsiSourceFile> { myFile };
    }

    public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return false;
    }

    public string ShortName
    {
      get { return myName; }
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
  }
}
