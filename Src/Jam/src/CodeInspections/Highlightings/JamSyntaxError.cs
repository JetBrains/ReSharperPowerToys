using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings;

[assembly: RegisterConfigurableSeverity(JamSyntaxError.Key, null, HighlightingGroupIds.LanguageUsage, JamSyntaxError.Name, @"Syntax error in JAM code", Severity.ERROR, false, Internal = false)]

namespace JetBrains.ReSharper.Psi.Jam.CodeInspections.Highlightings
{
  [ConfigurableSeverityHighlighting(Key, JamProjectFileType.Name, OverlapResolve = OverlapResolveKind.ERROR, ToolTipFormatString = Name)]
  internal class JamSyntaxError : IHighlightingWithRange, ICustomAttributeIdHighlighting
  {
    public const string Key = "JamSyntaxError";
    public const string Name = "Syntax error";
    private const string AtributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;

    private readonly DocumentRange myRange;
    private readonly string myDescription;

    public JamSyntaxError(DocumentRange range, string description = Name)
    {
      myRange = range;
      myDescription = description;
    }

    public string AttributeId
    {
      get { return AtributeId; }
    }

    public bool IsValid()
    {
      return true;
    }

    public string ToolTip
    {
      get { return myDescription; }
    }

    public string ErrorStripeToolTip
    {
      get { return myDescription; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public DocumentRange CalculateRange()
    {
      return myRange;
    }
  }
}
