using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Xml.Bulbs;
using JetBrains.ReSharper.Intentions.Xml.ContextActions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Parsing;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace XmlAndHtml
{
  /// <summary>
  /// This context action works on XML files. When you're on a tag that doesn't have an
  /// 'id' attribute, the context action creates it.
  /// </summary>
  [ContextAction(
    Group = "XML",
    Name = "Specify Id",
    Description = "Creates an 'id' attribute for the selected tag of an XML document",
    Priority = 0)]
  public class SpecifyIdXmlContextAction : XmlContextAction
  {
    public SpecifyIdXmlContextAction(XmlContextActionDataProvider dataProvider)
      : base(dataProvider) { }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      IXmlTagHeader tagHeader = GetTagHeader();
      if (tagHeader == null)
        return null;

      IXmlElementFactory factory = XmlElementFactory.GetInstance(tagHeader);

      IXmlAttribute idAttr = factory.CreateAttribute("id=\"\"", DataProvider.PsiServices, DataProvider.PsiModule);
      IXmlTag tag = tagHeader.GetContainingNode<IXmlTag>();
      if (tag == null)
        return null;

      tag.AddAttributeBefore(idAttr, null);

      // continuation to do after transaction commited
      return textControl =>
        // move cursor inside new created id attribute
        textControl.Caret.MoveTo(idAttr.Value.GetDocumentRange().TextRange.StartOffset, CaretVisualPlacement.Generic);
    }

    /// <summary>
    /// Returns the test that is rendered on the context action.
    /// </summary>
    public override string Text
    {
      get { return "Specify 'id'"; }
    }

    public override bool IsAvailable(IUserDataHolder dataHolder)
    {
      // grab the tag we're on
      IXmlTagHeader tagHeader = GetTagHeader();
      if (tagHeader == null)
        return false;

      // check if the attribute is already there (case-insensitive)
      IXmlAttribute idAtt = tagHeader.GetAttribute(attr => StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeName, "id"));
      if (idAtt != null)
        return false;

      return true;
    }

    private IXmlTagHeader GetTagHeader() { return DataProvider.FindNodeAtCaret<IXmlTagHeader>(); }
  }
}
