using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Impl.Tree;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  internal class JamParser : JamParserGenerated, IJamParser
  {
    private readonly SeldomInterruptChecker myCheckForInterrupt;
    private readonly ILexer myOriginalLexer;

    protected IPsiSourceFile SourceFile;

    protected JamParser(ILexer lexer)
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
      myOriginalLexer = lexer;
      setLexer(new FilteringJamLexer(lexer));
    }

    [NotNull]
    public IJamFile ParseFile()
    {
      return (IJamFile)PrepareElement(parseJamFile(), false);
    }

    public IJamDeclaration ParseDeclaration()
    {
      return (IJamDeclaration)PrepareElement(parseJamDeclaration(), false);
    }

    public IJamStatement ParseStatement()
    {
      return (IJamStatement)PrepareElement(parseJamStatement(), false);
    }

    public IJamExpression ParseExpression()
    {
      return (IJamExpression)PrepareElement(parseJamExpression(), false);
    }

    [NotNull]
    IFile IParser.ParseFile()
    {
      return ParseFile();
    }

    public override TreeElement parseJamExpression()
    {
      var leftExpression = ParseLeftExpression();

      var tokenType = myLexer.TokenType;

      if (tokenType == JamTokenType.PLUS || tokenType == JamTokenType.MINUS)
        return parseAdditiveExpression(leftExpression);

      if (tokenType == JamTokenType.MULTIPLY || tokenType == JamTokenType.DIVIDE)
        return parseMultiplicativeExpression(leftExpression);

      if ( tokenType == JamTokenType.EQEQ || tokenType == JamTokenType.NEQ || tokenType == JamTokenType.GT || tokenType == JamTokenType.LT || tokenType == JamTokenType.GE || tokenType == JamTokenType.LE)
        return parseConditionalExpression(leftExpression);

      return leftExpression;
    }

    public override TreeElement parseAdditiveExpression(TreeElement left)
    {
      if (left == null)
        left = ParseLeftExpression();

      var result = TreeElementFactory.CreateCompositeElement(ElementType.ADDITIVE_EXPRESSION);

      try
      {
        result.AppendNewChild(left);

        var tokenType = myLexer.TokenType;
        result.AppendNewChild(tokenType == TokenType.PLUS ? match(JamTokenType.PLUS) : match(JamTokenType.MINUS));

        result.AppendNewChild(parseJamExpression());
      }
      catch (SyntaxError e)
      {
        if (left != null && left.Parent == null)
        {
          if (result != null) result.AppendNewChild(left);
        }
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
          handleErrorInAdditiveExpression(result, e);
        }
        else
        {
          throw;
        }
      }

      return result;
    }

    public override TreeElement parseMultiplicativeExpression(TreeElement left)
    {
      if (left == null)
        left = ParseLeftExpression();

      var result = TreeElementFactory.CreateCompositeElement(ElementType.MULTIPLICATIVE_EXPRESSION);

      try
      {
        result.AppendNewChild(left);

        var tokenType = myLexer.TokenType;
        result.AppendNewChild(tokenType == TokenType.MULTIPLY ? match(JamTokenType.MULTIPLY) : match(JamTokenType.DIVIDE));

        result.AppendNewChild(parseJamExpression());
      }
      catch (SyntaxError e)
      {
        if (left != null && left.Parent == null)
        {
          if (result != null) result.AppendNewChild(left);
        }
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
          handleErrorInMultiplicativeExpression(result, e);
        }
        else
        {
          throw;
        }
      }

      return result;
    }

    public override TreeElement parseConditionalExpression(TreeElement left)
    {
      if (left == null)
        left = ParseLeftExpression();

      var result = TreeElementFactory.CreateCompositeElement(ElementType.CONDITIONAL_EXPRESSION);

      try
      {
        result.AppendNewChild(left);

        var tokenType = myLexer.TokenType;

        if (tokenType == TokenType.EQEQ)
          result.AppendNewChild(match(TokenType.EQEQ));
        else if (tokenType == TokenType.NEQ)
          result.AppendNewChild(match(TokenType.NEQ));
        else if (tokenType == TokenType.GT)
          result.AppendNewChild(match(TokenType.GT));
        else if (tokenType == TokenType.LT)
          result.AppendNewChild(match(TokenType.LT));
        else if (tokenType == TokenType.GE)
          result.AppendNewChild(match(TokenType.GE));
        else if (tokenType == TokenType.LE)
          result.AppendNewChild(match(TokenType.LE));
        else {
          if (result.firstChild == null) result = null;
          throw new UnexpectedToken (ParserMessages.GetExpectedMessage(ParserMessages.IDS_JAM_CONDITIONAL_OPERATOR));
        }

        result.AppendNewChild(parseJamExpression());
      }
      catch (SyntaxError e)
      {
        if (left != null && left.Parent == null)
        {
          if (result != null) result.AppendNewChild(left);
        }
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
          handleErrorInConditionalExpression(result, e);
        }
        else
        {
          throw;
        }
      }

      return result;
    }

    public override TreeElement parseGroupExpression()
    {
      var result = TreeElementFactory.CreateCompositeElement (ElementType.GROUP_EXPRESSION);
      try {
        result.AppendNewChild (match(TokenType.LPAREN));
        result.AppendNewChild (parseJamExpression());
        result.AppendNewChild (match(TokenType.RPAREN));
      } catch (SyntaxError e) {
        if (e.ParsingResult != null && result != null) {
          result.AppendNewChild (e.ParsingResult);
        }
        if (result != null) {
          e.ParsingResult = result;
        }
        if (result != null) {
          handleErrorInGroupExpression (result, e);
        } else {
          throw;
        }
      }
      return result;
    }

    [NotNull]
    public override TreeElement parseIdentifier()
    {
      TokenNodeType tokenType = myLexer.TokenType;

      if (tokenType == JamTokenType.IDENTIFIER)
        return CreateToken(JamTokenType.IDENTIFIER);

      throw new UnexpectedToken(ParserMessages.GetExpectedMessage(ParserMessages.GetString(ParserMessages.IDS__IDENTIFIER)));
    }

    private TreeElement ParseLeftExpression()
    {
      var tokenType = myLexer.TokenType;

      if (tokenType == JamTokenType.LPAREN)
        return parseGroupExpression();

      if (tokenType == JamTokenType.STRING_LITERAL)
        return parseStringLiteralExpression();

      if (tokenType == JamTokenType.IDENTIFIER)
        return myLexer.LookaheadToken(1) == JamTokenType.LPAREN ? parseInvocationExpression() : parseIdentifierExpression();

      return parseLiteralExpression();
    }

    [NotNull]
    private TreeElement PrepareElement([NotNull] TreeElement compositeElement, bool trimMissingTokens)
    {
      InsertMissingTokens(compositeElement, trimMissingTokens);
      return compositeElement;
    }

    private void InsertMissingTokens(TreeElement result, bool trimMissingTokens)
    {
      JamMissingTokensInserter.Run(result, myOriginalLexer, this, trimMissingTokens, myCheckForInterrupt);
    }

    [NotNull]
    protected override TreeElement createToken()
    {
      Assertion.Assert(myLexer.TokenType != null, "myLexer.TokenType != null");

      var element = myLexer.TokenType.Create(myLexer.Buffer, new TreeOffset(myLexer.TokenStart), new TreeOffset(myLexer.TokenEnd));
      SetOffset(element, myLexer.TokenStart);
      myLexer.Advance();
      return element;
    }

    [NotNull]
    private TreeElement CreateToken([NotNull] TokenNodeType tokenType)
    {
      Assertion.Assert(tokenType != null, "tokenType != null");

      LeafElementBase element;

      if (tokenType == JamTokenType.IDENTIFIER)
        element = new JamIdentifierToken(myLexer.GetCurrTokenText());
      else
        element = tokenType.Create(myLexer.Buffer, new TreeOffset(myLexer.TokenStart), new TreeOffset(myLexer.TokenEnd));

      SetOffset(element, myLexer.TokenStart);
      myLexer.Advance();

      return element;
    }
  }
}