// This is generated file, DO NOT EDIT
using JetBrains.ReSharper.PsiPlugin.Tree.Impl;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.PsiPlugin.Parsing
{
  public static partial class PsiTokenType
  {
    #region ABSTRACT
    private class AbstractNodeType : KeywordTokenNodeType
    {
      public AbstractNodeType(): base ("ABSTRACT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AbstractTokenElement();
      }
    }
    private class AbstractTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ABSTRACT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ABSTRACT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ABSTRACT = new AbstractNodeType();
    #endregion
    #region ERRORHANDLING
    private class ErrorhandlingNodeType : KeywordTokenNodeType
    {
      public ErrorhandlingNodeType(): base ("ERRORHANDLING") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ErrorhandlingTokenElement();
      }
    }
    private class ErrorhandlingTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ERRORHANDLING);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ERRORHANDLING;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ERRORHANDLING = new ErrorhandlingNodeType();
    #endregion
    #region EXTRAS
    private class ExtrasNodeType : KeywordTokenNodeType
    {
      public ExtrasNodeType(): base ("EXTRAS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ExtrasTokenElement();
      }
    }
    private class ExtrasTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(EXTRAS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return EXTRAS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType EXTRAS = new ExtrasNodeType();
    #endregion
    #region GET
    private class GetNodeType : KeywordTokenNodeType
    {
      public GetNodeType(): base ("GET") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new GetTokenElement();
      }
    }
    private class GetTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(GET);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return GET;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType GET = new GetNodeType();
    #endregion
    #region GETTER
    private class GetterNodeType : KeywordTokenNodeType
    {
      public GetterNodeType(): base ("GETTER") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new GetterTokenElement();
      }
    }
    private class GetterTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(GETTER);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return GETTER;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType GETTER = new GetterNodeType();
    #endregion
    #region OPTIONS
    private class OptionsNodeType : KeywordTokenNodeType
    {
      public OptionsNodeType(): base ("OPTIONS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new OptionsTokenElement();
      }
    }
    private class OptionsTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(OPTIONS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return OPTIONS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType OPTIONS = new OptionsNodeType();
    #endregion
    #region INTERFACE
    private class InterfaceNodeType : KeywordTokenNodeType
    {
      public InterfaceNodeType(): base ("INTERFACE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new InterfaceTokenElement();
      }
    }
    private class InterfaceTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(INTERFACE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return INTERFACE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType INTERFACE = new InterfaceNodeType();
    #endregion
    #region INTERFACES
    private class InterfacesNodeType : KeywordTokenNodeType
    {
      public InterfacesNodeType(): base ("INTERFACES") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new InterfacesTokenElement();
      }
    }
    private class InterfacesTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(INTERFACES);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return INTERFACES;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType INTERFACES = new InterfacesNodeType();
    #endregion
    #region ISCACHED
    private class IscachedNodeType : KeywordTokenNodeType
    {
      public IscachedNodeType(): base ("ISCACHED") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new IscachedTokenElement();
      }
    }
    private class IscachedTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ISCACHED);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ISCACHED;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ISCACHED = new IscachedNodeType();
    #endregion
    #region PRIVATE
    private class PrivateNodeType : KeywordTokenNodeType
    {
      public PrivateNodeType(): base ("PRIVATE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PrivateTokenElement();
      }
    }
    private class PrivateTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PRIVATE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PRIVATE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PRIVATE = new PrivateNodeType();
    #endregion
    #region PATHS
    private class PathsNodeType : KeywordTokenNodeType
    {
      public PathsNodeType(): base ("PATHS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PathsTokenElement();
      }
    }
    private class PathsTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PATHS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PATHS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PATHS = new PathsNodeType();
    #endregion
    #region RETURN_TYPE
    private class ReturnTypeNodeType : KeywordTokenNodeType
    {
      public ReturnTypeNodeType(): base ("RETURN_TYPE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ReturnTypeTokenElement();
      }
    }
    private class ReturnTypeTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(RETURN_TYPE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return RETURN_TYPE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType RETURN_TYPE = new ReturnTypeNodeType();
    #endregion
    #region ROLE_KEYWORD
    private class RoleKeywordNodeType : KeywordTokenNodeType
    {
      public RoleKeywordNodeType(): base ("ROLE_KEYWORD") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new RoleKeywordTokenElement();
      }
    }
    private class RoleKeywordTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ROLE_KEYWORD);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ROLE_KEYWORD;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ROLE_KEYWORD = new RoleKeywordNodeType();
    #endregion
    #region CACHED
    private class CachedNodeType : KeywordTokenNodeType
    {
      public CachedNodeType(): base ("CACHED") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new CachedTokenElement();
      }
    }
    private class CachedTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(CACHED);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return CACHED;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType CACHED = new CachedNodeType();
    #endregion
    #region NULL
    private class NullNodeType : KeywordTokenNodeType
    {
      public NullNodeType(): base ("NULL") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new NullTokenElement();
      }
    }
    private class NullTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(NULL);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return NULL;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType NULL = new NullNodeType();
    #endregion
    #region LIST_KEYWORD
    private class ListKeywordNodeType : KeywordTokenNodeType
    {
      public ListKeywordNodeType(): base ("LIST_KEYWORD") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ListKeywordTokenElement();
      }
    }
    private class ListKeywordTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LIST_KEYWORD);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LIST_KEYWORD;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LIST_KEYWORD = new ListKeywordNodeType();
    #endregion
    #region SEP_KEYWORD
    private class SepKeywordNodeType : KeywordTokenNodeType
    {
      public SepKeywordNodeType(): base ("SEP_KEYWORD") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new SepKeywordTokenElement();
      }
    }
    private class SepKeywordTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(SEP_KEYWORD);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return SEP_KEYWORD;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType SEP_KEYWORD = new SepKeywordNodeType();
    #endregion
    #region NULL_KEYWORD
    private class NullKeywordNodeType : KeywordTokenNodeType
    {
      public NullKeywordNodeType(): base ("NULL_KEYWORD") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new NullKeywordTokenElement();
      }
    }
    private class NullKeywordTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(NULL_KEYWORD);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return NULL_KEYWORD;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType NULL_KEYWORD = new NullKeywordNodeType();
    #endregion
    #region LPARENTH
    private class LparenthNodeType : GenericTokenNodeType
    {
      public LparenthNodeType(): base ("LPARENTH") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LparenthTokenElement();
      }
    }
    private class LparenthTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LPARENTH);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LPARENTH;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LPARENTH = new LparenthNodeType();
    #endregion
    #region RPARENTH
    private class RparenthNodeType : GenericTokenNodeType
    {
      public RparenthNodeType(): base ("RPARENTH") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new RparenthTokenElement();
      }
    }
    private class RparenthTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(RPARENTH);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return RPARENTH;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType RPARENTH = new RparenthNodeType();
    #endregion
    #region LBRACE
    private class LbraceNodeType : GenericTokenNodeType
    {
      public LbraceNodeType(): base ("LBRACE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LbraceTokenElement();
      }
    }
    private class LbraceTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LBRACE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LBRACE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LBRACE = new LbraceNodeType();
    #endregion
    #region RBRACE
    private class RbraceNodeType : GenericTokenNodeType
    {
      public RbraceNodeType(): base ("RBRACE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new RbraceTokenElement();
      }
    }
    private class RbraceTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(RBRACE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return RBRACE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType RBRACE = new RbraceNodeType();
    #endregion
    #region LBRACKET
    private class LbracketNodeType : GenericTokenNodeType
    {
      public LbracketNodeType(): base ("LBRACKET") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LbracketTokenElement();
      }
    }
    private class LbracketTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LBRACKET);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LBRACKET;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LBRACKET = new LbracketNodeType();
    #endregion
    #region RBRACKET
    private class RbracketNodeType : GenericTokenNodeType
    {
      public RbracketNodeType(): base ("RBRACKET") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new RbracketTokenElement();
      }
    }
    private class RbracketTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(RBRACKET);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return RBRACKET;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType RBRACKET = new RbracketNodeType();
    #endregion
    #region SEMICOLON
    private class SemicolonNodeType : GenericTokenNodeType
    {
      public SemicolonNodeType(): base ("SEMICOLON") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new SemicolonTokenElement();
      }
    }
    private class SemicolonTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(SEMICOLON);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return SEMICOLON;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType SEMICOLON = new SemicolonNodeType();
    #endregion
    #region COMMA
    private class CommaNodeType : GenericTokenNodeType
    {
      public CommaNodeType(): base ("COMMA") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new CommaTokenElement();
      }
    }
    private class CommaTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(COMMA);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return COMMA;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType COMMA = new CommaNodeType();
    #endregion
    #region DOT
    private class DotNodeType : GenericTokenNodeType
    {
      public DotNodeType(): base ("DOT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new DotTokenElement();
      }
    }
    private class DotTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(DOT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return DOT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType DOT = new DotNodeType();
    #endregion
    #region EQ
    private class EqNodeType : GenericTokenNodeType
    {
      public EqNodeType(): base ("EQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new EqTokenElement();
      }
    }
    private class EqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(EQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return EQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType EQ = new EqNodeType();
    #endregion
    #region GT
    private class GtNodeType : GenericTokenNodeType
    {
      public GtNodeType(): base ("GT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new GtTokenElement();
      }
    }
    private class GtTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(GT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return GT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType GT = new GtNodeType();
    #endregion
    #region LT
    private class LtNodeType : GenericTokenNodeType
    {
      public LtNodeType(): base ("LT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LtTokenElement();
      }
    }
    private class LtTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LT = new LtNodeType();
    #endregion
    #region EXCL
    private class ExclNodeType : GenericTokenNodeType
    {
      public ExclNodeType(): base ("EXCL") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ExclTokenElement();
      }
    }
    private class ExclTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(EXCL);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return EXCL;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType EXCL = new ExclNodeType();
    #endregion
    #region TILDE
    private class TildeNodeType : GenericTokenNodeType
    {
      public TildeNodeType(): base ("TILDE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new TildeTokenElement();
      }
    }
    private class TildeTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(TILDE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return TILDE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType TILDE = new TildeNodeType();
    #endregion
    #region SHARP
    private class SharpNodeType : GenericTokenNodeType
    {
      public SharpNodeType(): base ("SHARP") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new SharpTokenElement();
      }
    }
    private class SharpTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(SHARP);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return SHARP;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType SHARP = new SharpNodeType();
    #endregion
    #region AT
    private class AtNodeType : GenericTokenNodeType
    {
      public AtNodeType(): base ("AT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AtTokenElement();
      }
    }
    private class AtTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(AT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return AT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType AT = new AtNodeType();
    #endregion
    #region BACK_QUOTE
    private class BackQuoteNodeType : GenericTokenNodeType
    {
      public BackQuoteNodeType(): base ("BACK_QUOTE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new BackQuoteTokenElement();
      }
    }
    private class BackQuoteTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(BACK_QUOTE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return BACK_QUOTE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType BACK_QUOTE = new BackQuoteNodeType();
    #endregion
    #region QUEST
    private class QuestNodeType : GenericTokenNodeType
    {
      public QuestNodeType(): base ("QUEST") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new QuestTokenElement();
      }
    }
    private class QuestTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(QUEST);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return QUEST;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType QUEST = new QuestNodeType();
    #endregion
    #region COLON
    private class ColonNodeType : GenericTokenNodeType
    {
      public ColonNodeType(): base ("COLON") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ColonTokenElement();
      }
    }
    private class ColonTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(COLON);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return COLON;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType COLON = new ColonNodeType();
    #endregion
    #region PLUS
    private class PlusNodeType : GenericTokenNodeType
    {
      public PlusNodeType(): base ("PLUS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PlusTokenElement();
      }
    }
    private class PlusTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PLUS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PLUS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PLUS = new PlusNodeType();
    #endregion
    #region MINUS
    private class MinusNodeType : GenericTokenNodeType
    {
      public MinusNodeType(): base ("MINUS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new MinusTokenElement();
      }
    }
    private class MinusTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(MINUS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return MINUS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType MINUS = new MinusNodeType();
    #endregion
    #region ASTERISK
    private class AsteriskNodeType : GenericTokenNodeType
    {
      public AsteriskNodeType(): base ("ASTERISK") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AsteriskTokenElement();
      }
    }
    private class AsteriskTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ASTERISK);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ASTERISK;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ASTERISK = new AsteriskNodeType();
    #endregion
    #region DIV
    private class DivNodeType : GenericTokenNodeType
    {
      public DivNodeType(): base ("DIV") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new DivTokenElement();
      }
    }
    private class DivTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(DIV);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return DIV;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType DIV = new DivNodeType();
    #endregion
    #region AND
    private class AndNodeType : GenericTokenNodeType
    {
      public AndNodeType(): base ("AND") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AndTokenElement();
      }
    }
    private class AndTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(AND);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return AND;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType AND = new AndNodeType();
    #endregion
    #region OR
    private class OrNodeType : GenericTokenNodeType
    {
      public OrNodeType(): base ("OR") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new OrTokenElement();
      }
    }
    private class OrTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(OR);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return OR;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType OR = new OrNodeType();
    #endregion
    #region XOR
    private class XorNodeType : GenericTokenNodeType
    {
      public XorNodeType(): base ("XOR") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new XorTokenElement();
      }
    }
    private class XorTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(XOR);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return XOR;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType XOR = new XorNodeType();
    #endregion
    #region PERC
    private class PercNodeType : GenericTokenNodeType
    {
      public PercNodeType(): base ("PERC") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PercTokenElement();
      }
    }
    private class PercTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PERC);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PERC;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PERC = new PercNodeType();
    #endregion
    #region EQEQ
    private class EqeqNodeType : GenericTokenNodeType
    {
      public EqeqNodeType(): base ("EQEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new EqeqTokenElement();
      }
    }
    private class EqeqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(EQEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return EQEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType EQEQ = new EqeqNodeType();
    #endregion
    #region LE
    private class LeNodeType : GenericTokenNodeType
    {
      public LeNodeType(): base ("LE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LeTokenElement();
      }
    }
    private class LeTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LE = new LeNodeType();
    #endregion
    #region GE
    private class GeNodeType : GenericTokenNodeType
    {
      public GeNodeType(): base ("GE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new GeTokenElement();
      }
    }
    private class GeTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(GE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return GE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType GE = new GeNodeType();
    #endregion
    #region NE
    private class NeNodeType : GenericTokenNodeType
    {
      public NeNodeType(): base ("NE") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new NeTokenElement();
      }
    }
    private class NeTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(NE);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return NE;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType NE = new NeNodeType();
    #endregion
    #region ANDAND
    private class AndandNodeType : GenericTokenNodeType
    {
      public AndandNodeType(): base ("ANDAND") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AndandTokenElement();
      }
    }
    private class AndandTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ANDAND);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ANDAND;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ANDAND = new AndandNodeType();
    #endregion
    #region OROR
    private class OrorNodeType : GenericTokenNodeType
    {
      public OrorNodeType(): base ("OROR") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new OrorTokenElement();
      }
    }
    private class OrorTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(OROR);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return OROR;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType OROR = new OrorNodeType();
    #endregion
    #region PLUSPLUS
    private class PlusplusNodeType : GenericTokenNodeType
    {
      public PlusplusNodeType(): base ("PLUSPLUS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PlusplusTokenElement();
      }
    }
    private class PlusplusTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PLUSPLUS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PLUSPLUS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PLUSPLUS = new PlusplusNodeType();
    #endregion
    #region MINUSMINUS
    private class MinusminusNodeType : GenericTokenNodeType
    {
      public MinusminusNodeType(): base ("MINUSMINUS") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new MinusminusTokenElement();
      }
    }
    private class MinusminusTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(MINUSMINUS);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return MINUSMINUS;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType MINUSMINUS = new MinusminusNodeType();
    #endregion
    #region LTLT
    private class LtltNodeType : GenericTokenNodeType
    {
      public LtltNodeType(): base ("LTLT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LtltTokenElement();
      }
    }
    private class LtltTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LTLT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LTLT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LTLT = new LtltNodeType();
    #endregion
    #region GTGT
    private class GtgtNodeType : GenericTokenNodeType
    {
      public GtgtNodeType(): base ("GTGT") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new GtgtTokenElement();
      }
    }
    private class GtgtTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(GTGT);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return GTGT;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType GTGT = new GtgtNodeType();
    #endregion
    #region PLUSEQ
    private class PluseqNodeType : GenericTokenNodeType
    {
      public PluseqNodeType(): base ("PLUSEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PluseqTokenElement();
      }
    }
    private class PluseqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PLUSEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PLUSEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PLUSEQ = new PluseqNodeType();
    #endregion
    #region MINUSEQ
    private class MinuseqNodeType : GenericTokenNodeType
    {
      public MinuseqNodeType(): base ("MINUSEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new MinuseqTokenElement();
      }
    }
    private class MinuseqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(MINUSEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return MINUSEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType MINUSEQ = new MinuseqNodeType();
    #endregion
    #region ASTERISKEQ
    private class AsteriskeqNodeType : GenericTokenNodeType
    {
      public AsteriskeqNodeType(): base ("ASTERISKEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AsteriskeqTokenElement();
      }
    }
    private class AsteriskeqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ASTERISKEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ASTERISKEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ASTERISKEQ = new AsteriskeqNodeType();
    #endregion
    #region DIVEQ
    private class DiveqNodeType : GenericTokenNodeType
    {
      public DiveqNodeType(): base ("DIVEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new DiveqTokenElement();
      }
    }
    private class DiveqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(DIVEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return DIVEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType DIVEQ = new DiveqNodeType();
    #endregion
    #region ANDEQ
    private class AndeqNodeType : GenericTokenNodeType
    {
      public AndeqNodeType(): base ("ANDEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new AndeqTokenElement();
      }
    }
    private class AndeqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ANDEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ANDEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ANDEQ = new AndeqNodeType();
    #endregion
    #region OREQ
    private class OreqNodeType : GenericTokenNodeType
    {
      public OreqNodeType(): base ("OREQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new OreqTokenElement();
      }
    }
    private class OreqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(OREQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return OREQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType OREQ = new OreqNodeType();
    #endregion
    #region XOREQ
    private class XoreqNodeType : GenericTokenNodeType
    {
      public XoreqNodeType(): base ("XOREQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new XoreqTokenElement();
      }
    }
    private class XoreqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(XOREQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return XOREQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType XOREQ = new XoreqNodeType();
    #endregion
    #region PERCEQ
    private class PerceqNodeType : GenericTokenNodeType
    {
      public PerceqNodeType(): base ("PERCEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new PerceqTokenElement();
      }
    }
    private class PerceqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(PERCEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return PERCEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType PERCEQ = new PerceqNodeType();
    #endregion
    #region LTLTEQ
    private class LtlteqNodeType : GenericTokenNodeType
    {
      public LtlteqNodeType(): base ("LTLTEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LtlteqTokenElement();
      }
    }
    private class LtlteqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LTLTEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LTLTEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LTLTEQ = new LtlteqNodeType();
    #endregion
    #region GTGTEQ
    private class GtgteqNodeType : GenericTokenNodeType
    {
      public GtgteqNodeType(): base ("GTGTEQ") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new GtgteqTokenElement();
      }
    }
    private class GtgteqTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(GTGTEQ);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return GTGTEQ;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType GTGTEQ = new GtgteqNodeType();
    #endregion
    #region DOUBLE_COLON
    private class DoubleColonNodeType : GenericTokenNodeType
    {
      public DoubleColonNodeType(): base ("DOUBLE_COLON") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new DoubleColonTokenElement();
      }
    }
    private class DoubleColonTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(DOUBLE_COLON);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return DOUBLE_COLON;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType DOUBLE_COLON = new DoubleColonNodeType();
    #endregion
    #region DOUBLE_QUEST
    private class DoubleQuestNodeType : GenericTokenNodeType
    {
      public DoubleQuestNodeType(): base ("DOUBLE_QUEST") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new DoubleQuestTokenElement();
      }
    }
    private class DoubleQuestTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(DOUBLE_QUEST);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return DOUBLE_QUEST;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType DOUBLE_QUEST = new DoubleQuestNodeType();
    #endregion
    #region ARROW
    private class ArrowNodeType : GenericTokenNodeType
    {
      public ArrowNodeType(): base ("ARROW") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new ArrowTokenElement();
      }
    }
    private class ArrowTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(ARROW);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return ARROW;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType ARROW = new ArrowNodeType();
    #endregion
    #region LAMBDA_ARROW
    private class LambdaArrowNodeType : GenericTokenNodeType
    {
      public LambdaArrowNodeType(): base ("LAMBDA_ARROW") {}
      public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
      {
        return new LambdaArrowTokenElement();
      }
    }
    private class LambdaArrowTokenElement : PsiTokenBase
    {
      private static readonly string myTokenText = PsiLexer.GetTokenText(LAMBDA_ARROW);
      public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType
      {
        get { return LAMBDA_ARROW;}
      }
      public override int GetTextLength()
      {
        return myTokenText.Length;
      }
      public override string GetText()
      {
        return myTokenText;
      }
    }
    public static readonly TokenNodeType LAMBDA_ARROW = new LambdaArrowNodeType();
    #endregion
  }
}
