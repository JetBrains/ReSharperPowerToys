using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Cach
{
  /// <summary>
  /// Customization point for pdi properties
  /// </summary>
  [CannotApplyEqualityOperator]
  public interface IPsiSymbol : IPersistentCacheItem
  {
    /// <summary>
    /// Offset of symbol declaration in the source file tree
    /// </summary>
    int Offset { get; }
    /// <summary>
    /// Owner PSI source file
    /// </summary>
    IPsiSourceFile SourceFile { get; }
    /// <summary>
    /// Offset where name of symbol is specified
    /// </summary>
    int NavigationOffset { get; }
    /// <summary>
    /// DeclaredElementType element that is declared by this symbol
    /// </summary>
    DeclaredElementType GetElementType();
    /// <summary>
    /// Restore tree node that declares this symbol
    /// </summary>
    ITreeNode FindDeclaration(ITreeNode nodeAtOffset);
    /// <summary>
    /// Returns true for intentionally declared symbols (like var, {sss:} not just e = ???)
    /// </summary>
    bool IsExplicit();
  }

  public class PsiSymbol : IPsiSymbol
  {
    private string myText;
    private int myOffset;
    private IPsiSourceFile myPsiSourceFile;
    private StandartSymbolDeclarationKind myDeclarationKind;

    public const string Guid = "std-symbol";

    public PsiSymbol(ITreeNode treeNode)
    {
      if (treeNode is IRuleDeclaration)
      {
        myText = ((IRuleDeclaration)treeNode).DeclaredName;
      }
      else if(treeNode is IOptionDefinition)
      {
        var option = treeNode as IOptionDefinition;
        if(option.OptionIntegerValue != null)
        {
          myText = option.OptionName.GetText() + option.OptionIntegerValue.GetText();
        }
        if (option.OptionStringValue != null)
        {
          myText = option.OptionName.GetText() + option.OptionStringValue.GetText();
        }
        if (option.OptionIdentifierValue != null)
        {
          myText = option.OptionName.GetText() + option.OptionIdentifierValue.GetText();
        }
      } else
      {
        myText = treeNode.GetText();
      }
      if(myText == null)
      {
        myText = "";
      }
      myOffset = treeNode.GetNavigationRange().TextRange.StartOffset;
      myPsiSourceFile = treeNode.GetSourceFile();
      if(treeNode is RuleDeclaration)
      {
        myDeclarationKind = StandartSymbolDeclarationKind.Rule;
      } else if(treeNode is RoleName)
      {
        myDeclarationKind = StandartSymbolDeclarationKind.Role;
      }
      else if (treeNode is VariableDeclaration)
      {
        myDeclarationKind = StandartSymbolDeclarationKind.Variable;
      } else if(treeNode is OptionDefinition)
      {
        myDeclarationKind = StandartSymbolDeclarationKind.Option;
      } else if(treeNode is PathDeclaration)
      {
        myDeclarationKind = StandartSymbolDeclarationKind.Path;
      } 
    }

    public int Offset
    {
      get { return myOffset; }
    }

    public IPsiSourceFile SourceFile
    {
      get { return myPsiSourceFile; }
    }

    public int NavigationOffset
    {
      get { return Offset; }
    }

    public DeclaredElementType GetElementType()
    {
      if (myDeclarationKind == StandartSymbolDeclarationKind.Rule)
      {
        return PsiDeclaredElementType.Rule;
      }
      if (myDeclarationKind == StandartSymbolDeclarationKind.Role)
      {
        return PsiDeclaredElementType.Role;
      }
      if (myDeclarationKind == StandartSymbolDeclarationKind.Option)
      {
        return PsiDeclaredElementType.Option;
      }
      if (myDeclarationKind == StandartSymbolDeclarationKind.Variable)
      {
        return PsiDeclaredElementType.Variable;
      }
      if (myDeclarationKind == StandartSymbolDeclarationKind.Path)
      {
        return PsiDeclaredElementType.Path;
      }
      return null;
    }

    public ITreeNode FindDeclaration(ITreeNode nodeAtOffset)
    {
      while (nodeAtOffset != null)
      {
        if (IsDeclaration(nodeAtOffset))
          return nodeAtOffset;
        nodeAtOffset = nodeAtOffset.Parent;
      }
      return null;
    }

    private bool IsDeclaration(ITreeNode treeNode)
    {
      return ((treeNode is RuleDeclaration) || (treeNode is RoleName) || (treeNode is PathDeclaration) ||
              (treeNode is VariableDeclaration) || (treeNode is OptionDefinition));
    }

    public bool IsExplicit()
    {
      return true;
    }

    public void Write(BinaryWriter writer)
    {
      writer.Write(Text);
      writer.Write(Offset);
      writer.Write((int)myDeclarationKind);
    }

    public void Read(BinaryReader reader)
    {
      myText = reader.ReadString();
      myOffset = reader.ReadInt32();
      myDeclarationKind = (StandartSymbolDeclarationKind)reader.ReadInt32();
    }

    public string SymbolTypeGuid
    {
      get { return Guid; }
    }

    public string Text
    {
      get
      {
        return myText;
      }
    }
  }

  [Flags]
  public enum StandartSymbolDeclarationKind
  {
    // a : b;;
    Rule = 1,
    // a<PSI_A>;
    Role = 2,
    // options { a = b };
    Option = 4,
    // !(#a:A)
    Variable = 8,
    // <a:PSI_A>;
    Path = 16,
  }
}
