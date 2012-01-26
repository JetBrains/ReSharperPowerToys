using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  class OptionPropertyDeclaredElement : IDeclaredElement
  {
    private IPsiSourceFile myFile;
    private string myName;
    private IPsiServices myServices;

    public OptionPropertyDeclaredElement(IPsiSourceFile file, string name, IPsiServices services)
    {
      myFile = file;
      myName = name;
      myServices = services;
    }

    public IPsiServices GetPsiServices()
    {
      return myServices;
    }

    public IList<IDeclaration> GetDeclarations()
    {
      return EmptyList<IDeclaration>.InstanceList ;
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
      return new HybridCollection<IPsiSourceFile>() {myFile};
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
  }
}
