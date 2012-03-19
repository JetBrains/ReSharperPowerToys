using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  class RoleDeclaredElement : IDeclaredElement
  {
    private readonly IFile myFile;
    private string myName;
    private readonly IPsiServices myServices;
    private string myNewName;

    public RoleDeclaredElement(IFile file, string name, IPsiServices services)
    {
      myFile = file;
      myName = name;
      myNewName = name;
      myServices = services;
    }

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
      return PsiDeclaredElementType.Role;
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
      return new HybridCollection<IPsiSourceFile> {myFile.GetSourceFile()};
    }

    public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return sourceFile == myFile;
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

    public void SetName(string name)
    {
      myName = name;
    }

    public IFile File
    {
      get { return myFile; }
    }

    public bool ChangeName { get; set; }

    public string NewName
    {
      get { return myNewName; }
      set { ChangeName = true;
            myNewName = value;
      }
    }
  }
}
