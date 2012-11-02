using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Lexer.Psi;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing
{
  internal class PsiParser : PsiParserGenerated, IPsiParser
  {
    private readonly SeldomInterruptChecker myCheckForInterrupt;
    private readonly ILexer myOriginalLexer;
    protected IPsiSourceFile SourceFile;

    public PsiParser(ILexer lexer)
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
      myOriginalLexer = lexer;
      var tokenBuffer = new TokenBuffer(lexer);
      myLexer = new FilteringPsiLexer(tokenBuffer.CreateLexer());
      myLexer.Start();
    }

    #region IPsiParser Members

    public IFile ParseFile()
    {
      var file = (IFile)parsePsiFile();
      InsertMissingTokens((TreeElement)file, false);
      return file;
    }

    public TreeElement ParseStatement()
    {
      return PrepareElement(base.parseRuleDeclaration(), true);
    }

    #endregion

    private TreeElement PrepareElement(TreeElement compositeElement, bool trimMissingTokens)
    {
      if (!HasErrors(compositeElement))
      {
        InsertMissingTokens(compositeElement, trimMissingTokens);
      }
      return compositeElement;
    }

    private void InsertMissingTokens(TreeElement result, bool trimMissingTokens)
    {
      PsiMissingTokensInserter.Run(result, myOriginalLexer, this, trimMissingTokens, myCheckForInterrupt);
    }

    protected override TreeElement createToken()
    {
      LeafElementBase element = TreeElementFactory.CreateLeafElement(myLexer.TokenType, myLexer.Buffer, myLexer.TokenStart, myLexer.TokenEnd);
      SetOffset(element, myLexer.TokenStart);
      myLexer.Advance();
      return element;
    }

    public override TreeElement parsePsiFile()
    {
      return ParsePsiFile(true);
    }

    protected override CompositeElement handleErrorInRuleBody(CompositeElement result, SyntaxError error)
    {
      TokenNodeType tokenType;
      CompositeElement errorElement;
      if (result is IErrorElement)
      {
        errorElement = result;
      }
      else
      {
        errorElement = TreeElementFactory.CreateErrorElement(error.Message);
        result.AppendNewChild(errorElement);
      }
      tokenType = myLexer.TokenType;
      while (tokenType != null && tokenType != TokenType.SEMICOLON && !IsInFollow(tokenType) && (! PsiTokenType.KEYWORDS.Contains(tokenType)) && ( tokenType != PsiTokenType.IDENTIFIER))
      {
        skip(errorElement);
        tokenType = myLexer.TokenType;
      }
      return result;     
    }

    public override TreeElement parseRuleDeclaration()
    {
      TokenNodeType tokenType;
      CompositeElement result = null;
      TreeElement tempParsingResult = null;
      int startPosition = (myLexer as FilteringPsiLexer).CurrentPosition;
      try
      {
        result = TreeElementFactory.CreateCompositeElement(ElementType.RULE_DECLARATION);
        tokenType = myLexer.TokenType;
        while (tokenType != null && TokenBitsets.TokenBitset_5[tokenType])
        {
          tempParsingResult = parseModifier();
          result.AppendNewChild(tempParsingResult);
          tokenType = myLexer.TokenType;
        }
        tempParsingResult = parseRuleDeclaredName();
        result.AppendNewChild(tempParsingResult);
        tokenType = myLexer.TokenType;
        if (tokenType == TokenType.LBRACE)
        {
          tempParsingResult = parseRoleGetterParameter();
          result.AppendNewChild(tempParsingResult);
        }
        tokenType = myLexer.TokenType;
        if (tokenType == TokenType.LBRACKET)
        {
          tempParsingResult = parseRuleBracketTypedParameters();
          result.AppendNewChild(tempParsingResult);
        }
        tokenType = myLexer.TokenType;
        if (tokenType == TokenType.OPTIONS)
        {
          tempParsingResult = parseOptionsDefinition();
          result.AppendNewChild(tempParsingResult);
        }
        tokenType = myLexer.TokenType;
        if (tokenType == TokenType.EXTRAS)
        {
          tempParsingResult = parseExtrasDefinition();
          result.AppendNewChild(tempParsingResult);
        }
        tempParsingResult = match(TokenType.COLON);
        result.AppendNewChild(tempParsingResult);
        tempParsingResult = parseRuleBody();
        result.AppendNewChild(tempParsingResult);
        tempParsingResult = match(TokenType.SEMICOLON);
        result.AppendNewChild(tempParsingResult);
        tokenType = myLexer.TokenType;
        if (tokenType != null && !(TokenBitsets.TokenBitset_6[tokenType]))
        {
          throw new FollowsFailure(ErrorMessages.GetErrorMessage33());
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
          var endRuleDeclarationOffset = GetTokensLength(result);
          handleErrorInRuleDeclaration(result, e);
          if (result.FindFirstTokenIn() != result.FindLastTokenIn())
          {
            int errorRuleLength = TryParseMissedRule(result, startPosition);
            if (errorRuleLength < endRuleDeclarationOffset)
            {
              int currLength = 0;
              return TrimResult(result, errorRuleLength, currLength);
            }
          }
        }
        else
        {
          throw;
        }
      }
      return result;
    }

    private CompositeElement TrimResult(CompositeElement element, int endLength, int currLength)
    {
      var result = TreeElementFactory.CreateCompositeElement(ElementType.RULE_DECLARATION);
      var child = element.FirstChild;
      while(child != null)
      {
        var next = child.NextSibling;
        if(endLength < currLength + GetTokensLength(child))
        {
          AddMatchingChildren(result, child, endLength, currLength);
          break;
        } else
        {
          result.AppendNewChild(child as TreeElement);
          currLength += GetTokensLength(child);
        }
        child = next;
      }
      return result;
    }

    private int GetTokensLength(ITreeNode element)
    {
      if(element is ITokenNode)
      {
        return element.GetText().Length;
      }
      else
      {
        var child = element.FirstChild;
        int length = 0;
        while(child != null)
        {
          length += GetTokensLength(child);
          child = child.NextSibling;
        }
        return length;
      }
    }

    private void AddMatchingChildren(CompositeElement result, ITreeNode element, int endLength, int currLength)
    {
      var compositeElement = element as CompositeElement;
      if((compositeElement != null) && (compositeElement.FindFirstTokenIn() != compositeElement.FindLastTokenIn()))
      {
        var newElement = TreeElementFactory.CreateCompositeElement(compositeElement.NodeType as CompositeNodeType);
        var child = compositeElement.FirstChild;
        while(child != null)
        {
          var next = child.NextSibling;
          if (endLength <= currLength + GetTokensLength(child))
          {
            AddMatchingChildren(newElement, child, endLength, currLength);
            break;
          }
          else
          {
            newElement.AppendNewChild((child as TreeElement));
            currLength += GetTokensLength(child);
          }
          child = next;         
        }
        result.AppendNewChild(newElement);
      } else
      {
        var errorElement = TreeElementFactory.CreateErrorElement("unexpected end of rule declaration");
        result.AppendNewChild(errorElement);
      }
    }

    private int TryParseMissedRule(CompositeElement result, int startPosition)
    {
      var filteringPsiLexer = (myLexer as FilteringPsiLexer);
      int endPosition = filteringPsiLexer.CurrentPosition;
      filteringPsiLexer.Advance(startPosition - endPosition);
      int errorRuleLength = filteringPsiLexer.TokenEnd - filteringPsiLexer.TokenStart;
      filteringPsiLexer.Advance(1);
      var currentPosition = filteringPsiLexer.CurrentPosition;
      while ((filteringPsiLexer.TokenType == PsiTokenType.NEW_LINE) || (filteringPsiLexer.TokenType == PsiTokenType.WHITE_SPACE) || (filteringPsiLexer.TokenType == PsiTokenType.END_OF_LINE_COMMENT) || filteringPsiLexer.TokenType == PsiTokenType.C_STYLE_COMMENT)
      {
        filteringPsiLexer.Advance(1);
        currentPosition++;
      }
      int offset = filteringPsiLexer.TokenStart;
      var ruleDeclaration = base.parseRuleDeclaration();
      while((HasErrors(ruleDeclaration)) && (filteringPsiLexer.CurrentPosition < endPosition))
      {
        filteringPsiLexer.Advance(currentPosition - filteringPsiLexer.CurrentPosition);
        currentPosition++;
        while ((filteringPsiLexer.TokenType == PsiTokenType.NEW_LINE) || (filteringPsiLexer.TokenType == PsiTokenType.WHITE_SPACE) || (filteringPsiLexer.TokenType == PsiTokenType.END_OF_LINE_COMMENT) || filteringPsiLexer.TokenType == PsiTokenType.C_STYLE_COMMENT)
        {
          filteringPsiLexer.Advance(1);
          currentPosition++;
        }
        if ((!filteringPsiLexer.TokenType.IsWhitespace) && (!filteringPsiLexer.TokenType.IsComment))
        {
          errorRuleLength += filteringPsiLexer.TokenEnd - filteringPsiLexer.TokenStart;
        }
        ruleDeclaration = base.parseRuleDeclaration();
      }
      filteringPsiLexer.Advance(currentPosition - 1 - filteringPsiLexer.CurrentPosition);
      if(HasErrors(ruleDeclaration))
      {
        filteringPsiLexer.Advance(1);
      }
      return errorRuleLength;
    }

    private bool HasErrors(ITreeNode treeNode)
    {
      var child = treeNode.FirstChild;
      while(child != null)
      {
        if(child is IErrorElement)
        {
          return true;
        }
        if(HasErrors(child))
        {
          return true;
        }
        child = child.NextSibling;
      }
      return false;
    }

    protected override CompositeElement handleErrorInRuleDeclaration(CompositeElement result, SyntaxError error)
    {
      TokenNodeType tokenType;
      CompositeElement errorElement;
      if (result is IErrorElement)
      {
        errorElement = result;
      }
      else
      {
        errorElement = TreeElementFactory.CreateErrorElement(error.Message);
        result.AppendNewChild(errorElement);
      }
      tokenType = myLexer.TokenType;
      while (tokenType != null && !(TokenBitsets.TokenBitset_6[tokenType]) && !IsInFollow(tokenType) && (!PsiTokenType.KEYWORDS.Contains(tokenType)) && (tokenType != PsiTokenType.IDENTIFIER))
      {
        skip(errorElement);
        tokenType = myLexer.TokenType;
      }
      return result;
    }

    public TreeElement ParsePsiFile(bool isFileReal)
    {
      TreeElement file = base.parsePsiFile();
      var psiFile = file as PsiFile;
      if (psiFile != null)
      {
        psiFile.SetSourceFile(SourceFile);
        psiFile.CreateRulesSymbolTable();
        psiFile.CreateOptionSymbolTable(isFileReal);
      }
      return psiFile;
    }
  }
}
