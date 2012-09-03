using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application;
using JetBrains.ReSharper.LexPlugin.Lexer.Lex;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing
{
  internal class LexParser : LexParserGenerated, ILexParser
  {
    private readonly SeldomInterruptChecker myCheckForInterrupt;
    private readonly ILexer myOriginalLexer;
    protected IPsiSourceFile SourceFile;
    private bool myHasLBrace = false;

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
      }
      return lexFile;
    }

    public override TreeElement parseCsharpOrToken()
    {
      TokenNodeType tokenType;
      CompositeElement result = null;
      TreeElement tempParsingResult = null;
      try
      {
        result = TreeElementFactory.CreateCompositeElement(ElementType.CSHARP_OR_TOKEN);
        tempParsingResult = match(TokenType.LBRACE);
        result.AppendNewChild(tempParsingResult);
        tokenType = myLexer.TokenType;
        if (tokenType == TokenType.IDENTIFIER)
        {
          var tempParsingResult1 = parseTokenTypeName();
          tokenType = myLexer.TokenType;
          if (tokenType == TokenType.RBRACE)
          {
            result.AppendNewChild(tempParsingResult1);
          } else
          {
            var node = TreeElementFactory.CreateCompositeElement(ElementType.CSHARP_IDENTIFIER);
            node.AddChild(tempParsingResult1.FirstChild);
            tempParsingResult = parseCSharpBlock(node);
            result.AppendNewChild(tempParsingResult);
          }
        }
        else
        {
          tempParsingResult = parseCSharpBlock();
          result.AppendNewChild(tempParsingResult);
        }
        tempParsingResult = match(TokenType.RBRACE);
        result.AppendNewChild(tempParsingResult);
      }
      catch (SyntaxError e)
      {
        if (e.ParsingResult != null && result != null)
        {
          result.AppendNewChild(e.ParsingResult);
        }
        if (result != null)
        {
          e.ParsingResult = result;
        }
        throw;
      }
      return result;
    }

    private TreeElement parseCSharpBlock(TreeElement treeElement)
    {
      TokenNodeType tokenType;
      CompositeElement result = null;
      TreeElement tempParsingResult = null;
      try
      {
        result = TreeElementFactory.CreateCompositeElement(ElementType.C_SHARP_BLOCK);
        result.AppendNewChild(treeElement);
        tokenType = myLexer.TokenType;
        while (tokenType != null && TokenBitsets.TokenBitset_2[tokenType])
        {
          tokenType = myLexer.TokenType;
          if (tokenType != null && TokenBitsets.TokenBitset_3[tokenType])
          {
            tempParsingResult = parseCSharpToken();
            result.AppendNewChild(tempParsingResult);
          }
          else if (tokenType == TokenType.IDENTIFIER)
          {
            tempParsingResult = parseCsharpIdentifier();
            result.AppendNewChild(tempParsingResult);
          }
          else if (tokenType == TokenType.NULL_KEYWORD
          || tokenType == TokenType.RETURN_KEYWORD)
          {
            tempParsingResult = parseCsharpKeyword();
            result.AppendNewChild(tempParsingResult);
          }
          else if (tokenType == TokenType.LBRACE)
          {
            tempParsingResult = parseCsharpBraceExpression();
            result.AppendNewChild(tempParsingResult);
          }
          else
          {
            tempParsingResult = parseCSharpToken();
            result.AppendNewChild(tempParsingResult);
          }
          tokenType = myLexer.TokenType;
        }
        tokenType = myLexer.TokenType;
        if (tokenType != null && !(tokenType == JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl.TokenType.PERC
            || tokenType == JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree.Impl.TokenType.RBRACE))
        {
          throw new JetBrains.ReSharper.Psi.Parsing.FollowsFailure(ErrorMessages.GetErrorMessage5());
        }
      }
      catch (JetBrains.ReSharper.Psi.Parsing.SyntaxError e)
      {
        if (e.ParsingResult != null && result != null)
        {
          result.AppendNewChild(e.ParsingResult);
        }
        if (result != null)
        {
          e.ParsingResult = result;
        }
        if (result != null)
        {
          handleErrorInCSharpBlock(result, e);
        }
        else
        {
          throw;
        }
      }
      return result;
    }
  }
}
