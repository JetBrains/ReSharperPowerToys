using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Lexer.Lex;
using JetBrains.Text;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.PsiPlugin.LexGrammar
{
  [ProjectFileType(typeof (LexProjectFileType))]
  public class LexProjectFileLanguageService : ProjectFileLanguageService
  {
    public LexProjectFileLanguageService(LexProjectFileType projectFileType)
      : base(projectFileType)
    {
    }

    protected override PsiLanguageType PsiLanguageType
    {
      get { return LexLanguage.Instance; }
    }

    public override IconId Icon
    {
      get
      {
        //return LexPluginSymbolThemedIcons.PsiFile.Id;
        return null;
      }
    }

    public override ILexerFactory GetMixedLexerFactory(ISolution solution, IBuffer buffer, IPsiSourceFile sourceFile = null)
    {
      {
        return new LexLexerFactory();
      }
    }
  }
}
