using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public class FileAssociationViewController : TreeViewController
  {
    public override bool ExpandNodeInitially(TreeModelViewNode viewNode)
    {
      return true;
    }

    public override bool VisibilityState(TreeModelNode modelNode)
    {
      return true;
    }

    public override bool DragDropSupported { get { return false; } }

    public override bool GuardActions { get { return true; } }

    public override bool QuickSearchSupported { get { return false; } }
  }
}