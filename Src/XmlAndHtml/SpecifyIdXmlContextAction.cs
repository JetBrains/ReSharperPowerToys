using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Xml.Bulbs;
using JetBrains.ReSharper.Intentions.Xml.ContextActions;
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
    private IXmlTag myTag;

    public SpecifyIdXmlContextAction(XmlContextActionDataProvider dataProvider)
      : base(dataProvider) { }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      IXmlElementFactory factory = XmlElementFactory.GetElementFactory(myTag);
      string text = String.Format("id=\"\"");

      IXmlFile file = factory.CreateFile(DataProvider.PsiServices, DataProvider.PsiModule,
                                          "<tag " + text + "/>", true);
      IXmlAttribute att = file.InnerTags.First().GetAttributes().First();
      myTag.AddAttributeBefore(att, null);
      return null;
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
      var tag = DataProvider.FindNodeAtCaret<IXmlTag>();
      if (tag == null)
        return false;

      // check if the attribute is already there
      var idAtt = tag.GetAttribute("id");
      if (idAtt != null)
        return false;

      // if there's no such attribute, save the tag and return
      myTag = tag;
      return true;
    }
  }
}
