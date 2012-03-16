using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Lexer;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
 internal class PsiParser : PsiParserGenerated, IPsiParser, ITokenOffsetProvider
  {
    private ILexer myOriginalLexer;
    private SeldomInterruptChecker myCheckForInterrupt;
    protected IPsiSourceFile mySourceFile;

    public PsiParser()
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
    }

    public PsiParser(IPsiSourceFile  sourceFile)
    {
      myCheckForInterrupt = new SeldomInterruptChecker();
      mySourceFile = sourceFile;

    }

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
      var file = (IFile) parsePsiFile();
      InsertMissingTokens((TreeElement) file, false);
      return file;
    }

   public TreeElement ParseStatement()
   {
     return PrepareElement(base.parseRuleDeclaration(), true);
   }

   private TreeElement PrepareElement(TreeElement compositeElement, bool trimMissingTokens)
   {
     InsertMissingTokens(compositeElement, trimMissingTokens);
     return compositeElement;
   }

   #endregion

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
      return parsePsiFile(true);
    }

    public TreeElement parsePsiFile(bool isFileReal)
    {
      var file = base.parsePsiFile();
      PsiFile psiFile = (PsiFile) file;
      psiFile.SetSourceFile(mySourceFile);
      psiFile.CreateRulesSymbolTable();
      psiFile.CreateOptionSymbolTable(isFileReal);
      return psiFile;
    }

  }
}