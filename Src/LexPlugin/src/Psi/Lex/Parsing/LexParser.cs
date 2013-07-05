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
using JetBrains.ReSharper.Psi.Util;

namespace JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing
{
  internal class LexParser : LexParserGenerated, ILexParser
  {
    private readonly SeldomInterruptChecker myCheckForInterrupt;
    private readonly ILexer myOriginalLexer;
    protected IPsiSourceFile SourceFile;
    private CommonIdentifierIntern myCommonIdentifierIntern;
    private ITokenIntern myTokenIntern;

    public LexParser(ILexer lexer, CommonIdentifierIntern commonIdentifierIntern)
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
      myOriginalLexer = lexer;
      myCommonIdentifierIntern = commonIdentifierIntern;
      myLexer = new FilteringLexLexer(lexer);
      myLexer.Start();
    }

    #region ILexParser Members

    public IFile ParseFile()
    {
      return myCommonIdentifierIntern.DoWithIdentifierIntern(intern =>
      {
        myTokenIntern = intern;
        var file = (LexFile)parseLexFile();
        InsertMissingTokens(file, false, intern);
        myTokenIntern = null;
        return file;
      });
    }

    public TreeElement ParseStatement()
    {
      return myCommonIdentifierIntern.DoWithIdentifierIntern(intern =>
      {
        return PrepareElement(base.parseLexFile(), true, intern);
      });
    }

    #endregion

    private TreeElement PrepareElement(TreeElement compositeElement, bool trimMissingTokens, ITokenIntern intern)
    {
      InsertMissingTokens(compositeElement, trimMissingTokens, intern);
      return compositeElement;
    }

    private void InsertMissingTokens(TreeElement result, bool trimMissingTokens, ITokenIntern intern)
    {
      LexMissingTokensInserter.Run(result, myOriginalLexer, this, trimMissingTokens, myCheckForInterrupt, intern);
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
        lexFile.CollectIncluded();
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
        if (tokenType != null && !(tokenType == TokenType.PERC
            || tokenType == TokenType.RBRACE))
        {
          throw new FollowsFailure(ErrorMessages.GetErrorMessage5());
        }
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
