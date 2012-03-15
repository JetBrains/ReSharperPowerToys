using System;
using System.IO;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Cache
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
  }

  public class PsiSymbol : IPsiSymbol
  {
    private string myName;
    private string myValue;
    private int myOffset;
    private readonly IPsiSourceFile myPsiSourceFile;
    private StandartSymbolDeclarationKind myDeclarationKind;

    private const string Guid = "std-symbol";

    public PsiSymbol(ITreeNode treeNode)
    {
      myValue = "";
      if (treeNode is IRuleDeclaration)
      {
        myName = ((IRuleDeclaration)treeNode).DeclaredName;
      }
      else if(treeNode is IOptionDefinition)
      {
        var option = treeNode as IOptionDefinition;
        myName = option.OptionName.GetText();
        if(option.OptionIntegerValue != null)
        {
          myValue = option.OptionIntegerValue.GetText();
        }
        if (option.OptionStringValue != null)
        {
          myValue = option.OptionStringValue.GetText();
        }
        if (option.OptionIdentifierValue != null)
        {
          myValue = option.OptionIdentifierValue.GetText();
        }
        if(myValue.Length > 0)
        {
          if("\"".Equals(myValue.Substring(0,1)))
          {
            myValue = myValue.Substring(1, myValue.Length - 1);
          }
          if ("\"".Equals(myValue.Substring(myValue.Length - 1, 1)))
          {
            myValue = myValue.Substring(0, myValue.Length - 1);
          }
        }
      } else
      {
        myName = treeNode.GetText();
      }
      if(myName == null)
      {
        myName = "";
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

    public void Write(BinaryWriter writer)
    {
      writer.Write(Name);
      writer.Write(Value);
      writer.Write(Offset);
      writer.Write((int)myDeclarationKind);
    }

    public void Read(BinaryReader reader)
    {
      myName = reader.ReadString();
      myValue = reader.ReadString();
      myOffset = reader.ReadInt32();
      myDeclarationKind = (StandartSymbolDeclarationKind)reader.ReadInt32();
    }

    public string SymbolTypeGuid
    {
      get { return Guid; }
    }

    public string Name
    {
      get
      {
        return myName;
      }
    }

    public string Value
    {
      get
      {
        return myValue;
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
