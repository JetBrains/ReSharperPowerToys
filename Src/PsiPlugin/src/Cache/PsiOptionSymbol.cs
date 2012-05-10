using System.IO;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  public class PsiOptionSymbol : IPsiSymbol
  {
    private readonly IPsiSourceFile myPsiSourceFile;
    private string myName;
    private int myOffset;
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

    public string Name
    {
      get { return myName; }
    }

    public string Value
    {
      get { return myValue; }
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
