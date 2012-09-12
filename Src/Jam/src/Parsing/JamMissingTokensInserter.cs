using System;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Jam.Impl.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Jam.Parsing
{
  internal sealed class JamMissingTokensInserter : MissingTokenInserterBase
  {
    private readonly ILexer myLexer;
    private new readonly DataIntern<string> myWhitespaceIntern = new DataIntern<string>();

    private JamMissingTokensInserter(ILexer lexer, ITokenOffsetProvider offsetProvider, SeldomInterruptChecker interruptChecker) : base(offsetProvider, interruptChecker)
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
        var anchor = element;
        var parent = anchor.parent;
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
        ((CachingLexer) myLexer).FindTokenAt(leafEndOffset);
      }
      else
      {
        while (myLexer.TokenType != null && myLexer.TokenStart < leafEndOffset)
          myLexer.Advance();
      }
    }

    private LeafElementBase CreateMissingToken()
    {
      var tokenType = myLexer.TokenType;
      if (tokenType == JamTokenType.WHITE_SPACE || tokenType == JamTokenType.NEW_LINE)
      {
        var text = myLexer.GetCurrTokenText();
        if (tokenType == JamTokenType.WHITE_SPACE)
          return new JamWhitespaceToken(myWhitespaceIntern.Intern(text));

        return new JamNewLineToken(text);
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

      var inserter = new JamMissingTokensInserter(lexer, offsetProvider, interruptChecker);
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
        get { return JamLanguage.Instance; }
      }

      public void Dispose()
      {
        DeleteChildRange(firstChild, firstChild);
      }

      private sealed class DummyNodeType : CompositeNodeType
      {
        public static readonly NodeType Instance = new DummyNodeType();

        private DummyNodeType() : base("DummyContainer") {}

        //public override PsiLanguageType LanguageType { get { return UnknownLanguage.Instance; } }
        public override CompositeElement Create()
        {
          throw new InvalidOperationException();
        }
      }
    }

    private sealed class EofToken : BindedToBufferLeafElement
    {
      public EofToken(IBuffer buffer) : base(JamTokenType.EOF, buffer, new TreeOffset(buffer.Length), new TreeOffset(buffer.Length)) {}

      public override PsiLanguageType Language
      {
        get { return JamLanguage.Instance; }
      }
    }
  }
}