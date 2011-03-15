using System;
using System.Globalization;
using System.IO;
using Reflector.CodeModel;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public class TextFormatter : IFormatter
  {
    private readonly StringWriter myWriter = new StringWriter(CultureInfo.InvariantCulture);
    private bool myAllowProperties;
    private int myIndent;
    private bool myNewLine;

    public bool AllowProperties
    {
      set { myAllowProperties = value; }
      get { return myAllowProperties; }
    }

    #region IFormatter Members

    public void Write(string text)
    {
      ApplyIndent();
      myWriter.Write(text);
    }

    public void WriteDeclaration(string text)
    {
      Write(text);
    }

    public void WriteDeclaration(string text, object target)
    {
      Write(text);
    }

    public void WriteComment(string text)
    {
      Write(text);
    }

    public void WriteLiteral(string text)
    {
      Write(text);
    }

    public void WriteKeyword(string text)
    {
      Write(text);
    }

    public void WriteIndent()
    {
      myIndent++;
    }

    public void WriteLine()
    {
      myWriter.WriteLine();
      myNewLine = true;
    }

    public void WriteOutdent()
    {
      myIndent--;
    }

    public void WriteReference(string text, string toolTip, Object reference)
    {
      ApplyIndent();
      myWriter.Write(text);
    }

    public void WriteProperty(string propertyName, string propertyValue)
    {
      if (myAllowProperties)
        throw new NotSupportedException();
    }

    #endregion

    public override string ToString()
    {
      return myWriter.ToString();
    }

    private void ApplyIndent()
    {
      if (myNewLine)
      {
        for (int i = 0; i < myIndent; i++)
          myWriter.Write("  ");

        myNewLine = false;
      }
    }
  }
}