/*

Portions of this program (the ZenCoding Engine) were taken from
the Einar Egilsson blog post (http://tech.einaregilsson.com/2009/11/12/zen-coding-visual-studio-addin/)

Those parts are copyright (C) 2009 Einar Egilsson (http://tech.einaregilsson.com/)

Portions of this program (the ZenCoding Python library) were taken from
the ZenCoding project (http://code.google.com/p/zen-coding/)

Those parts are copyright (C) 2009 Sergey Chikuyonok (http://chikuyonok.ru)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
$Id: ZenCodingEngine.cs 295 2009-11-12 21:46:48Z einar@einaregilsson.com $
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using IronPython.Hosting;
using IronPython.Runtime;
using JetBrains.Application;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ZenCoding
{
  public enum DocType
  {
    None = 0,
    Html,
    Css,
    Xsl
  }

  [ShellComponent]
  public class ZenCodingEngine
  {
    private readonly Func<string, string, string> expandAbbr;
    private readonly Func<string, string, string, string> wrapWithAbbr;
    private readonly Func<string, int, PythonTuple> findAbbrInLine;
    public Func<string, int, string> PadString { get; private set; }

    private const string InsertionPoint = "$IP$";

    public ZenCodingEngine()
    {
      ScriptEngine engine = Python.CreateEngine();
      ScriptScope scope = engine.CreateScope();
      string folder = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
      var searchPaths = new List<string>(engine.GetSearchPaths())
      {
        Path.Combine(folder, "zencoding"), Path.Combine(folder, "sys")
      };
      engine.SetSearchPaths(searchPaths);

      var code = new StringBuilder()
        .AppendLine("import zen_settings")
        .AppendLine("import zen_core")
        .AppendLine("from zen_core import *")
        .AppendLine("from zen_settings import zen_settings")
        .AppendFormat("zen_core.insertion_point = '{0}'", InsertionPoint)
          .AppendLine()
        .AppendFormat("zen_settings['variables']['indentation'] = '{0}'", "  ")
          .AppendLine()
        .AppendFormat("zen_core.newline = '{0}'", Regex.Escape(Environment.NewLine))
          .AppendLine();
      ScriptSource source = engine.CreateScriptSourceFromString(code.ToString(), SourceCodeKind.Statements);
      source.Execute(scope);
      expandAbbr = engine.GetVariable<Func<string, string, string>>(scope, "expand_abbreviation");
      PadString = engine.GetVariable<Func<string, int, string>>(scope, "pad_string");
      findAbbrInLine = engine.GetVariable<Func<string, int, PythonTuple>>(scope, "find_abbr_in_line");
      wrapWithAbbr = engine.GetVariable<Func<string, string, string, string>>(scope, "wrap_with_abbreviation");
    }

    public string ExpandAbbreviation(string abbreviation, DocType docType)
    {
      return expandAbbr(abbreviation, docType.ToString().ToLowerInvariant());
    }

    public string ExpandAbbreviation(string abbreviation, DocType docType, out int relativeInsertionPoint)
    {
      return ProcessInsertPoint(ExpandAbbreviation(abbreviation, docType), out relativeInsertionPoint);
    }

    private static string ProcessInsertPoint(string text, out int relativeInsertionPoint)
    {
      relativeInsertionPoint = -1;
      if (text.IsEmpty())
        return text;
      relativeInsertionPoint = text.IndexOf(InsertionPoint);
      return text.Replace(InsertionPoint, "");
    }

    public string FindAbbreviationInLine(string line, int index, out int startIndex)
    {
      PythonTuple tuple = findAbbrInLine(line, index);
      var abbreviation = (string)tuple[0];
      startIndex = string.IsNullOrEmpty(abbreviation) ? -1 : (int) tuple[1];
      return string.IsNullOrEmpty(abbreviation) ? null : abbreviation;
    }

    public string WrapWithAbbreviation(string abbreviation, string text, DocType docType)
    {
      return wrapWithAbbr(abbreviation, text, docType.ToString().ToLowerInvariant());
    }

    public string WrapWithAbbreviation(string abbreviation, string text, DocType docType, out int relativeInsertionPoint)
    {
      return ProcessInsertPoint(WrapWithAbbreviation(abbreviation, text, docType), out relativeInsertionPoint);
    }
  }
}