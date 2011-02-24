using JetBrains.CommonControls;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.TreeModels;
using JetBrains.UI.RichText;
using JetBrains.UI.TreeView;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public class FileAssociationPresenter : TreeModelBrowserPresenter
  {
    protected override void PresentObject(object value, IPresentableItem item, TreeModelNode modelNode, PresentationState state)
    {
      var association = value as FileAssociation;
      if (association != null)
      {
        RichText richText = association.Pattern ?? "(empty)";
        item.RichText = richText;
      }
    }
  }
}