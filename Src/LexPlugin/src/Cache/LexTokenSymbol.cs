using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.LexPlugin.Cache
{
  public class LexTokenSymbol : ILexSymbol
  {
    private readonly IPsiSourceFile myPsiSourceFile;
    private string myName;
    private int myOffset;

    public LexTokenSymbol(IPsiSourceFile psiSourceFile)
    {
      myPsiSourceFile = psiSourceFile;
    }

    public LexTokenSymbol(string name, int offset, IPsiSourceFile psiSourceFile)
    {
      myName = name;
      myOffset = offset;
      myPsiSourceFile = psiSourceFile;
    }

    public string Name
    {
      get { return myName; }
    }

    #region IPsiSymbol Members

    public int Offset
    {
      get { return myOffset; }
    }

    public IPsiSourceFile SourceFile
    {
      get { return myPsiSourceFile; }
    }

    #endregion

    public void Write(BinaryWriter writer)
    {
      writer.Write(Name);
      writer.Write(Offset);
    }

    public void Read(BinaryReader reader)
    {
      myName = reader.ReadString();
      myOffset = reader.ReadInt32();
    }
  }
}
