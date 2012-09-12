using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.Jam.Impl;

/*
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_KEYWORD, EffectType = EffectType.TEXT, ForegroundColor = "Blue", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_COMMENT, EffectType = EffectType.TEXT, ForegroundColor = "Green", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_STRING_LITERAL, EffectType = EffectType.TEXT, ForegroundColor = "Yellow", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_NUMERIC_LITERAL, EffectType = EffectType.TEXT, ForegroundColor = "Black", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_LOCAL_VARIABLE_NAME, EffectType = EffectType.TEXT, ForegroundColor = "Black", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_GLOBAL_VARIABLE_NAME, EffectType = EffectType.TEXT, ForegroundColor = "Black", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_PROCEDURE_NAME, EffectType = EffectType.TEXT, ForegroundColor = "Black", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
[assembly: RegisterHighlighter(JamHighlightingAttributeIds.JAM_PARAMETER_NAME, EffectType = EffectType.TEXT, ForegroundColor = "Black", Layer = HighlighterLayer.SYNTAX, VSPriority = VSPriority.IDENTIFIERS)]
*/

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings
{
  public static class JamHighlightingAttributeIds
  {
// ReSharper disable InconsistentNaming
    public const string JAM_KEYWORD = "ReSharper Template Editor C# Keyword" /*"ReSharper JAM keyword"*/;
    public const string JAM_COMMENT = "ReSharper Template Editor C# Comment" /*"ReSharper JAM comment"*/;
    public const string JAM_STRING_LITERAL = "ReSharper ASP.NET MVC Area" /*"ReSharper JAM string literal"*/;
    public const string JAM_NUMERIC_LITERAL = HighlightingAttributeIds.EVENT_IDENTIFIER_ATTRIBUTE /*"ReSharper JAM numeric literal"*/;
    public const string JAM_GLOBAL_VARIABLE_NAME = HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE /*"ReSharper JAM global variable name"*/;
    public const string JAM_LOCAL_VARIABLE_NAME = HighlightingAttributeIds.LOCAL_VARIABLE_IDENTIFIER_ATTRIBUTE /*"ReSharper JAM local variable name"*/;
    public const string JAM_PROCEDURE_NAME = HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE /*"ReSharper JAM procedure name"*/;
    public const string JAM_PARAMETER_NAME = HighlightingAttributeIds.PARAMETER_IDENTIFIER_ATTRIBUTE /*"ReSharper JAM parameter name"*/;
// ReSharper restore InconsistentNaming

    private static readonly Dictionary<DeclaredElementType, string> ElemenetTypeToHighlightingAttributeId= new Dictionary<DeclaredElementType, string>(4)
      {
        {JamDeclaredElementType.GlobalVariable, JAM_GLOBAL_VARIABLE_NAME},
        {JamDeclaredElementType.LocalVariable, JAM_LOCAL_VARIABLE_NAME},
        {JamDeclaredElementType.Parameter, JAM_PARAMETER_NAME},
        {JamDeclaredElementType.Procedure, JAM_PROCEDURE_NAME},
      };

    [CanBeNull]
    public static string GetAttributeId([NotNull] DeclaredElementType elementType)
    {
      string value;
      if (ElemenetTypeToHighlightingAttributeId.TryGetValue(elementType, out value))
        return value;

      return null;
    }
  }
}