using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;
using System.Linq;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  internal abstract class JamDeclaredElementBase<T> : IJamDeclaredElement where T : class, IJamDeclaration
  {
    [NotNull] private readonly string myShortName;
    private readonly TreeOffset myNameStartOffset;
    [NotNull] private readonly IPsiSourceFile myPsiSourceFile;

    [NotNull] private WeakReference<T> myDeclaration;

    protected JamDeclaredElementBase(T declaration)
    {
      myShortName = declaration.DeclaredName;
      myNameStartOffset = declaration.GetNameRange().StartOffset;

      var psiSourceFile = declaration.GetSourceFile();
      Assertion.AssertNotNull(psiSourceFile, "myPsiSourceFile != null");
      myPsiSourceFile = psiSourceFile;

      myDeclaration = new WeakReference<T>(declaration);
    }

    public PsiLanguageType PresentationLanguage { get { return JamLanguage.Instance; }}

    public string ShortName
    {
      get { return myShortName; }
    }

    public bool IsSynthetic() { return false; }

    public bool CaseSensistiveName { get { return true; } }

    public abstract DeclaredElementType GetElementType();

    public virtual XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    public virtual XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      return null;
    }

    public IPsiServices GetPsiServices()
    {
      return myPsiSourceFile.GetPsiServices();
    }

    public bool IsValid()
    {
      return GetDeclarations().All(declaration => declaration.IsValid());
    }

    public IList<IDeclaration> GetDeclarations()
    {
      var declaration = GetDeclaration();
      if (declaration == null)
        return EmptyList<IDeclaration>.InstanceList;

      return new List<IDeclaration> {declaration};
    }

    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return sourceFile == myPsiSourceFile ? GetDeclarations() : EmptyList<IDeclaration>.InstanceList;
    }

    public HybridCollection<IPsiSourceFile> GetSourceFiles()
    {
      return new HybridCollection<IPsiSourceFile>(myPsiSourceFile);
    }

    public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return sourceFile == myPsiSourceFile;
    }

    [CanBeNull]
    protected T GetDeclaration()
    {
      var declaration = myDeclaration.Target;

      if (declaration == null)
      {
        declaration = TryToFindDeclaration();

        if (declaration != null)
          myDeclaration = new WeakReference<T>(declaration);
      }
      return declaration;
    }

    [CanBeNull]
    private T TryToFindDeclaration()
    {
      var jamFile = myPsiSourceFile.GetNonInjectedPsiFile<JamLanguage>() as IJamFile;
      if (jamFile == null)
        return null;

      var identifier = jamFile.FindTokenAt(myNameStartOffset) as IJamIdentifier;
      if (identifier == null || !ShortName.Equals(identifier.Name, StringComparison.Ordinal))
        return null;

      return GetDeclaration(identifier);
    }

    [CanBeNull]
    protected abstract T GetDeclaration([NotNull] IJamIdentifier identifier);
  }
}