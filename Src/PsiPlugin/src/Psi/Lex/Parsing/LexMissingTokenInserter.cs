using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.LexGrammar;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Lex.Parsing
{
  internal sealed class LexMissingTokensInserter : MissingTokenInserterBase
  {
    private readonly ILexer myLexer;
    private new readonly DataIntern<string> myWhitespaceIntern = new DataIntern<string>();

    private LexMissingTokensInserter(ILexer lexer, ITokenOffsetProvider offsetProvider, SeldomInterruptChecker interruptChecker)
      : base(offsetProvider, interruptChecker)
    {
      myLexer = lexer;
    }

    protected override void ProcessLeafElement(TreeElement element)
    {
      int leafOffset = GetLeafOffset(element).Offset;

      // Check if some tokens are missed before this leaf
      if (myLexer.TokenType != null && myLexer.TokenStart < leafOffset)
      {
        // Find out the right place to insert tokens to
        TreeElement anchor = element;
        CompositeElement parent = anchor.parent;
        while (anchor == parent.firstChild && parent.parent != null)
        {
          anchor = parent;
          parent = parent.parent;
        }

        // proceed with inserting tokens
        while (myLexer.TokenType != null && myLexer.TokenStart < leafOffset)
        {
          LeafElementBase token = CreateMissingToken();

          parent.AddChildBefore(token, anchor);

          myLexer.Advance();
        }
      }

      // skip all tokens which lie inside given leaf element
      int leafEndOffset = leafOffset + element.GetTextLength();
      if ((element is IClosedChameleonBody) && (myLexer is CachingLexer))
      {
        ((CachingLexer)myLexer).FindTokenAt(leafEndOffset);
      }
      else
      {
        while (myLexer.TokenType != null && myLexer.TokenStart < leafEndOffset)
        {
          myLexer.Advance();
        }
      }
    }

    private LeafElementBase CreateMissingToken()
    {
      TokenNodeType tokenType = myLexer.TokenType;
      if (tokenType == LexTokenType.WHITE_SPACE || tokenType == LexTokenType.NEW_LINE)
      {
        string text = myLexer.GetCurrTokenText();
        if (tokenType == LexTokenType.WHITE_SPACE)
        {
          return new Whitespace(myWhitespaceIntern.Intern(text));
        }
        return new NewLine(text);
      }

      return TreeElementFactory.CreateLeafElement(myLexer);
    }

    public static void Run(TreeElement node, ILexer lexer, ITokenOffsetProvider offsetProvider, bool trimTokens, SeldomInterruptChecker interruptChecker)
    {
      Assertion.Assert(node.parent == null, "node.parent == null");

      var root = node as CompositeElement;
      if (root == null)
      {
        return;
      }

      var inserter = new LexMissingTokensInserter(lexer, offsetProvider, interruptChecker);
      lexer.Start();

      if (trimTokens)
      {
        using (var container = new DummyContainer(root))
        {
          inserter.Run(container);
        }
      }
      else
      {
        var terminator = new EofToken(lexer.Buffer);
        root.AppendNewChild(terminator);
        inserter.Run(root);
        root.DeleteChildRange(terminator, terminator);
      }
    }

    #region Nested type: DummyContainer

    private sealed class DummyContainer : CompositeElement, IDisposable
    {
      public DummyContainer(TreeElement element)
      {
        AppendNewChild(element);
      }

      public override NodeType NodeType
      {
        get { return DummyNodeType.Instance; }
      }

      public override PsiLanguageType Language
      {
        get { return LexLanguage.Instance; }
      }

      #region IDisposable Members

      public void Dispose()
      {
        DeleteChildRange(firstChild, firstChild);
      }

      #endregion

      #region Nested type: DummyNodeType

      private sealed class DummyNodeType : CompositeNodeType
      {
        public static readonly NodeType Instance = new DummyNodeType();

        private DummyNodeType()
          : base("DummyContainer")
        {
        }

        //public override PsiLanguageType LanguageType { get { return UnknownLanguage.Instance; } }
        public override CompositeElement Create()
        {
          throw new InvalidOperationException();
        }
      }

      #endregion
    }

    #endregion

    #region Nested type: EofToken

    private sealed class EofToken : BindedToBufferLeafElement
    {
      public EofToken(IBuffer buffer)
        : base(LexTokenType.EOF, buffer, new TreeOffset(buffer.Length), new TreeOffset(buffer.Length))
      {
      }

      public override PsiLanguageType Language
      {
        get { return LexLanguage.Instance; }
      }
    }

    #endregion
  }
}
