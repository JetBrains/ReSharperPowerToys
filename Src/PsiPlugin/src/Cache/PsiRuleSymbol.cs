using System.IO;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Cache
{
  public class PsiRuleSymbol : IPsiSymbol
  {
    private string myName;
    private int myOffset;
    private readonly IPsiSourceFile myPsiSourceFile;

    public PsiRuleSymbol(IPsiSourceFile psiSourceFile)
    {
      myPsiSourceFile = psiSourceFile;
    }

    public PsiRuleSymbol(string name, int offset, IPsiSourceFile psiSourceFile)
    {
      myName = name;
      myOffset = offset;
      myPsiSourceFile = psiSourceFile;
    }

    private const string Guid = "std-symbol";


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
      writer.Write(Offset);
    }

    public void Read(BinaryReader reader)
    {
      myName = reader.ReadString();
      myOffset = reader.ReadInt32();
    }

    public string Name
    {
      get
      {
        return myName;
      }
    }
  }
}