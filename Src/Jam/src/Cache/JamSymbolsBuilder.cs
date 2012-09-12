using System;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Jam.Cache
{
  internal static class JamSymbolsBuilder 
  {
    public static CompactOneToListMap<string, IJamSymbol> Build(IJamFile jamFile)
    {
      var psiSourceFile = jamFile.GetSourceFile();
      Assertion.AssertNotNull(psiSourceFile, "psiSourceFile is null");
      
      var map = new CompactOneToListMap<string, IJamSymbol>(StringComparer.Ordinal);
      jamFile.ProcessThisAndDescendants(new Visitor(psiSourceFile), map);

      map.Compact();
      return map;
    }

    private class Visitor : TreeNodeVisitor<CompactOneToListMap<string, IJamSymbol>>, IRecursiveElementProcessor<CompactOneToListMap<string, IJamSymbol>>
    {
      private readonly IPsiSourceFile myPsiSourceFile;

      public Visitor(IPsiSourceFile psiSourceFile)
      {
        myPsiSourceFile = psiSourceFile;
      }

      #region IRecursiveElementProcessor

      public bool InteriorShouldBeProcessed(ITreeNode element, CompactOneToListMap<string, IJamSymbol> context)
      {
        return true;
      }

      public bool IsProcessingFinished(CompactOneToListMap<string, IJamSymbol> context)
      {
        return false;
      }

      public void ProcessBeforeInterior(ITreeNode element, CompactOneToListMap<string, IJamSymbol> context) {}

      public void ProcessAfterInterior(ITreeNode element, CompactOneToListMap<string, IJamSymbol> context)
      {
        var jamTreeNode = element as IJamTreeNode;
        if (jamTreeNode != null)
          jamTreeNode.Accept(this, context);
      }

      #endregion

      public override void VisitGlobalVariableDeclaration(IGlobalVariableDeclaration globalVariableDeclarationParam, CompactOneToListMap<string, IJamSymbol> context)
      {
        if (globalVariableDeclarationParam.Name != null)
        {
          var jamSymbol = new JamSymbol(JamSymbolType.GlobalVariable, globalVariableDeclarationParam.Name.GetTreeStartOffset().Offset, globalVariableDeclarationParam.DeclaredName, myPsiSourceFile);
          context.AddValue(jamSymbol.Name, jamSymbol);
        }

        base.VisitGlobalVariableDeclaration(globalVariableDeclarationParam, context);
      }

      public override void VisitProcedureDeclaration(IProcedureDeclaration procedureDeclarationParam, CompactOneToListMap<string, IJamSymbol> context)
      {
        if (procedureDeclarationParam.Name != null)
        {
          var jamSymbol = new JamSymbol(JamSymbolType.Procedure, procedureDeclarationParam.Name.GetTreeStartOffset().Offset, procedureDeclarationParam.DeclaredName, myPsiSourceFile);
          context.AddValue(jamSymbol.Name, jamSymbol);
        }

        base.VisitProcedureDeclaration(procedureDeclarationParam, context);
      }
    }
  }
}