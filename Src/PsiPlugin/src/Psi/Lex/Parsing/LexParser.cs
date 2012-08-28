using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Lexer.Lex;
using JetBrains.ReSharper.PsiPlugin.Psi.Lex.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing
{
  internal class LexParser : LexParserGenerated, ILexParser
  {
    private readonly SeldomInterruptChecker myCheckForInterrupt;
    private readonly ILexer myOriginalLexer;
    protected IPsiSourceFile SourceFile;

    public LexParser(ILexer lexer)
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
      myOriginalLexer = lexer;
      myLexer = new FilteringLexLexer(lexer);
      myLexer.Start();
    }

    #region ILexParser Members

    public IFile ParseFile()
    {
      var file = (IFile)parseLexFile();
      InsertMissingTokens((TreeElement)file, false);
      return file;
    }

    public TreeElement ParseStatement()
    {
      return PrepareElement(base.parseLexFile(), true);
    }

    #endregion

    private TreeElement PrepareElement(TreeElement compositeElement, bool trimMissingTokens)
    {
      InsertMissingTokens(compositeElement, trimMissingTokens);
      return compositeElement;
    }

    private void InsertMissingTokens(TreeElement result, bool trimMissingTokens)
    {
      LexMissingTokensInserter.Run(result, myOriginalLexer, this, trimMissingTokens, myCheckForInterrupt);
    }

    protected override TreeElement createToken()
    {
      LeafElementBase element = TreeElementFactory.CreateLeafElement(myLexer.TokenType, myLexer.Buffer, myLexer.TokenStart, myLexer.TokenEnd);
      SetOffset(element, myLexer.TokenStart);
      myLexer.Advance();
      return element;
    }

    public override TreeElement parseLexFile()
    {
      return ParseLexFile(true);
    }

    public TreeElement ParseLexFile(bool isFileReal)
    {
      TreeElement file = base.parseLexFile();
      var lexFile = file as LexFile;
      if (lexFile != null)
      {
        lexFile.SetSourceFile(SourceFile);
        lexFile.CollectOptions();
        //lexFile.CreateRulesSymbolTable();
        //lexFile.CreateOptionSymbolTable(isFileReal);
      }
      return lexFile;
    }
  }
}
