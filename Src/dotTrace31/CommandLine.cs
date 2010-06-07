using System;
using System.Text;

namespace JetBrains.ReSharper.PowerToys.dotTrace31
{
  public static class CommandLine
  {
    public static string QuoteIfNeed(string arg)
    {
      if (arg.IndexOf(' ') >= 0)
        return '"' + arg.Replace("\"", "\\\"") + '"';
      return arg.Replace("\"", "\\\"");
    }

    public static string ToString(string [] args)
    {
      int pos = 0;
      int count = args.Length - 0;
      if (count < 0)
        throw  new ArgumentException("Invalid argument");
      var builder = new StringBuilder();
      while (count-- > 0)
      {
        string arg = args[pos++];
        if (builder.Length > 0)
          builder.Append(' ');
        if (arg.Length == 0)
          throw new ArgumentException("Invalid argument size");
        builder.Append(QuoteIfNeed(arg));
      }
      return builder.ToString();
    }
  }
}