using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;

namespace JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl
{
  internal partial class OptionStringValue
  {
    private bool myInitReference;
    private IReference myReference;

    #region IOptionStringValue Members

    public override ReferenceCollection GetFirstClassReferences()
    {
      var option = Parent as OptionDefinition;
      if (option != null)
      {
        if (OptionDeclaredElements.DirectoryOptions.Contains(option.OptionName.GetText()))
        {
          if (!myInitReference)
          {
            myReference = new PsiFileReference<OptionStringValue, PsiTokenBase>(this, null, (PsiTokenBase)FirstChild,
              new TreeTextRange(
                new TreeOffset(1),
                new TreeOffset(
                  GetTextLength() - 1)));
            myInitReference = true;
          }
          return new ReferenceCollection(myReference);
        }
      }
      return ReferenceCollection.Empty;
    }

    #endregion
  }
}
