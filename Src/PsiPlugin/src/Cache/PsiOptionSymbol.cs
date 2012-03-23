﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  public class PsiOptionSymbol : IPsiSymbol
  {

    private string myName;
    private int myOffset;
    private readonly IPsiSourceFile myPsiSourceFile;
    private string myValue;

    public PsiOptionSymbol(IPsiSourceFile psiSourceFile)
    {
      myPsiSourceFile = psiSourceFile;
    }

    public PsiOptionSymbol(string name, int offset, string value, IPsiSourceFile psiSourceFile)
    {
      myName = name;
      myOffset = offset;
      myValue = value;
      myPsiSourceFile = psiSourceFile;
    }

    public int Offset
    {
      get { return myOffset; }
    }

    public IPsiSourceFile SourceFile
    {
      get {return myPsiSourceFile; }
    }

    public string Name
    {
      get { return myName; }
    }

    public string Value
    {
      get { return myValue; }
    }

    public void Write(BinaryWriter writer)
    {
      writer.Write(Name);
      writer.Write(Offset);
      writer.Write(Value);
    }

    public void Read(BinaryReader reader)
    {
      myName = reader.ReadString();
      myOffset = reader.ReadInt32();
      myValue = reader.ReadString();
    }

  }
}
