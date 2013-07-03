using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.PsiPlugin.Lexer.Psi;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing
{
  internal class PsiParser : PsiParserGenerated, IPsiParser
  {
    private readonly SeldomInterruptChecker myCheckForInterrupt;
    private readonly CommonIdentifierIntern myCommonIdentifierIntern;
    private readonly ILexer myOriginalLexer;
    protected IPsiSourceFile SourceFile;
    private ITokenIntern myTokenIntern;

    public PsiParser(ILexer lexer, CommonIdentifierIntern commonIdentifierIntern)
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
      myOriginalLexer = lexer;
      myCommonIdentifierIntern = commonIdentifierIntern;
      var tokenBuffer = new TokenBuffer(lexer);
      myLexer = new FilteringPsiLexer(tokenBuffer.CreateLexer());
      myLexer.Start();
    }

    #region IPsiParser Members

    public IFile ParseFile()
    {
      return myCommonIdentifierIntern.DoWithIdentifierIntern(intern =>
      {
        myTokenIntern = intern;
        var file = (PsiFile) parsePsiFile();
        InsertMissingTokens(file, false, intern);
        myTokenIntern = null;
        return file;
      });
    }

    public TreeElement ParseStatement()
    {
      return myCommonIdentifierIntern.DoWithIdentifierIntern(intern => { return PrepareElement(base.parseRuleDeclaration(), true, intern); });
    }

    #endregion

    private TreeElement PrepareElement(TreeElement compositeElement, bool trimMissingTokens, ITokenIntern intern)
    {
      InsertMissingTokens(compositeElement, trimMissingTokens, intern);
      return compositeElement;
    }

    private void InsertMissingTokens(TreeElement result, bool trimMissingTokens, ITokenIntern intern)
    {
      PsiMissingTokensInserter.Run(result, myOriginalLexer, this, trimMissingTokens, myCheckForInterrupt, intern);
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
      while (tokenType != null && tokenType != TokenType.SEMICOLON && !IsInFollow(tokenType) && (! PsiTokenType.KEYWORDS.Contains(tokenType)) && (tokenType != PsiTokenType.IDENTIFIER))
      {
        skip(errorElement);
        tokenType = myLexer.TokenType;
      }
      return result;
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
