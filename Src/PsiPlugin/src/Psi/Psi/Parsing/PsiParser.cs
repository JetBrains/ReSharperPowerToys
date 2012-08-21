using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Lexer;
using JetBrains.ReSharper.PsiPlugin.Lexer.Psi;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;

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
      myLexer = new FilteringPsiLexer(lexer);
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
      InsertMissingTokens(compositeElement, trimMissingTokens);
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
