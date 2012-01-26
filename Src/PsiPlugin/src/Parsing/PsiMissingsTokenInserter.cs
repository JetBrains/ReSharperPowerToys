using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
  internal sealed class PsiMissingTokensInserter : MissingTokenInserterBase
  {
    private readonly ILexer myLexer;
    private readonly DataIntern<string> myWhitespaceIntern = new DataIntern<string>();

    private PsiMissingTokensInserter(ILexer lexer, ITokenOffsetProvider offsetProvider, SeldomInterruptChecker interruptChecker)
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

        //DocCommentBlock docCommentBlock = null;
        var isDocumentable = parent is IPsiFile;
        TreeElement oldSon = null;

        // proceed with inserting tokens
        while (myLexer.TokenType != null && myLexer.TokenStart < leafOffset)
        {
          var token = CreateMissingToken();

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
          myLexer.Advance();
      }
    }

    private LeafElementBase CreateMissingToken()
    {
      TokenNodeType tokenType = myLexer.TokenType;
      if (tokenType == PsiTokenType.WHITE_SPACE || tokenType == PsiTokenType.NEW_LINE)
      {
        string text = myLexer.GetCurrTokenText();
        if (tokenType == PsiTokenType.WHITE_SPACE)
          return new Whitespace(myWhitespaceIntern.Intern(text));
        return new NewLine(text);
      }

      return TreeElementFactory.CreateLeafElement(myLexer);
    }

    private sealed class EofToken : BindedToBufferLeafElement
    {
      public EofToken(IBuffer buffer)
        : base(PsiTokenType.EOF, buffer, new TreeOffset(buffer.Length), new TreeOffset(buffer.Length))
      {
      }
    }

    public static void Run(TreeElement node, ILexer lexer, ITokenOffsetProvider offsetProvider, bool trimTokens, SeldomInterruptChecker interruptChecker)
    {
      Assertion.Assert(node.parent == null, "node.parent == null");

      var root = node as CompositeElement;
      if (root == null)
        return;

      var inserter = new PsiMissingTokensInserter(lexer, offsetProvider, interruptChecker);
      lexer.Start();

      if (trimTokens)
      {
        using (var container = new DummyContainer(lexer.Buffer, root))
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

    private sealed class DummyContainer : CompositeElement, IDisposable
    {
      private sealed class DummyNodeType : CompositeNodeType
      {
        public static readonly NodeType Instance = new DummyNodeType();

        private DummyNodeType() : base("DummyContainer") { }
        public override PsiLanguageType LanguageType { get { return UnknownLanguage.Instance; } }
        public override CompositeElement Create() { throw new InvalidOperationException(); }
      }

      public DummyContainer(IBuffer buffer, TreeElement element)
      {
        AppendNewChild(element);
        //AppendNewChild(new EofToken(buffer));
      }

      public override NodeType NodeType
      {
        get { return DummyNodeType.Instance; }
      }

      public void Dispose()
      {
        DeleteChildRange(firstChild, firstChild);
      }
    }
  }
}
