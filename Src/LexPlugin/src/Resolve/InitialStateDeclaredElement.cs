using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.LexPlugin.Resolve
{
  public class InitialStateDeclaredElement : IDeclaredElement
  {
    private readonly IPsiSourceFile myFile;
    private const string Name = "YYINITIAL";
    private readonly IPsiServices myServices;

    public InitialStateDeclaredElement(IPsiSourceFile file, IPsiServices services)
    {
      myFile = file;
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
      return LexDeclaredElementType.State;
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
      get { return Name; }
    }

    public bool CaseSensistiveName
    {
      get { return true; }
    }

    public PsiLanguageType PresentationLanguage
    {
      get { return LexLanguage.Instance; }
    }

    #endregion
  }
}
