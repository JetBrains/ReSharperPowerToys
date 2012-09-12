using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Jam.Cache
{
  internal class JamSymbol : IJamSymbol 
  {
    public JamSymbol(JamSymbolType symbolType, int offset, [NotNull] string name, [NotNull] IPsiSourceFile psiSourceFile)
    {
      if (name == null) throw new ArgumentNullException("name");
      if (psiSourceFile == null) throw new ArgumentNullException("psiSourceFile");

      SymbolType = symbolType;
      Offset = offset;
      Name = name;
      PsiSourceFile = psiSourceFile;
    }

    public JamSymbolType SymbolType { get; private set; }
    public int Offset { get; private set; }
    public string Name { get; private set; }
    public IPsiSourceFile PsiSourceFile { get; private set; }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((JamSymbol) obj);
    }

    protected bool Equals(JamSymbol other)
    {
      return SymbolType.Equals(other.SymbolType) && Offset == other.Offset && string.Equals(Name, other.Name) && PsiSourceFile.Equals(other.PsiSourceFile);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int hashCode = SymbolType.GetHashCode();
        hashCode = (hashCode*397) ^ Offset;
        hashCode = (hashCode*397) ^ Name.GetHashCode();
        hashCode = (hashCode*397) ^ PsiSourceFile.GetHashCode();
        return hashCode;
      }
    }

    public IDeclaration GetDeclaration()
    {
      var jamFile = PsiSourceFile.GetNonInjectedPsiFile<JamLanguage>() as IJamFile;
      if (jamFile == null) return null;

      var tokenAt = jamFile.FindTokenAt(PsiSourceFile.Document, Offset);
      if (tokenAt == null)
        return null;

      var identifier = tokenAt.GetContainingNode<IJamIdentifier>(true);
      if (identifier == null)
        return null;

      switch (SymbolType)
      {
        case JamSymbolType.GlobalVariable:
          return GlobalVariableDeclarationNavigator.GetByName(identifier);
        case JamSymbolType.Procedure:
          return ProcedureDeclarationNavigator.GetByName(identifier);
      }

      return null;
    }
  }
}