using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Html.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Html.Parsing;
using JetBrains.ReSharper.Psi.Html.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using System.Linq;

namespace XmlAndHtml
{
  [ContextAction(Group = "HTML", Name = "Specify Id", Description = "Creates an 'id' attribute for the selected tag of an HTML document")]
  public class SpecifyIdHtmlContextAction : IContextAction, IBulbItem
  {
    private readonly IWebContextActionDataProvider<IHtmlFile> myProvider;
    private IHtmlTag myTag;

    public SpecifyIdHtmlContextAction(IWebContextActionDataProvider<IHtmlFile> provider)
    {
      myProvider = provider;
    }

    public bool IsAvailable(IUserDataHolder cache)
    {
      var tag = myProvider.FindNodeAtCaret<IHtmlTag>();
      if (tag == null)
        return false;

      var idAtt = tag.Attributes.FirstOrDefault(a => a.AttributeName.Equals("id"));
      if (idAtt == null)
      {
        myTag = tag;
        return true;
      }

      return false;
    }

    public IBulbItem[] Items
    {
      get { return new IBulbItem[] { this }; }
    }

    public void Execute(ISolution solution, ITextControl textControl)
    {
      // The easiest way to create an attribute is to create an HTML tag with an attribute in it
      // and then get the attribute from the tag.

      var psiServices = solution.GetComponent<IPsiServices>();
      using (new PsiTransactionCookie(psiServices, DefaultAction.Commit, Text)) 
      {
        var factory = HtmlElementFactory.GetInstance(myTag.Language);
        var dummy = factory.CreateHtmlTag("<tag id=\"\"/>", myTag);
        myTag.AddAttributeBefore(dummy.Attributes.First(), null);
      }
    }

    public string Text
    {
      get { return "Specify 'id'"; }
    }
  }
}