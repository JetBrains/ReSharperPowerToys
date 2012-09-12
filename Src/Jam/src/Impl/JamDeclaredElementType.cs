using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Jam.Resources;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.Psi.Jam.Impl
{
  public class JamDeclaredElementType : DeclaredElementTypeBase 
  {
    public static readonly JamDeclaredElementType Procedure = new JamDeclaredElementType("procedure", JamSymbolThemedIcons.Procedure.Id);
    public static readonly JamDeclaredElementType LocalVariable = new JamDeclaredElementType("local variable", JamSymbolThemedIcons.LocalVar.Id);
    public static readonly JamDeclaredElementType GlobalVariable = new JamDeclaredElementType("global variable", JamSymbolThemedIcons.GlobalVar.Id);
    public static readonly JamDeclaredElementType Parameter = new JamDeclaredElementType("parameter", JamSymbolThemedIcons.Parameter.Id);

    public JamDeclaredElementType(string name, [CanBeNull] IconId imageName) : base(name, imageName) {}

    protected override IDeclaredElementPresenter DefaultPresenter { get { return JamDeclaredElementPresenter.Instance; } }
  }
}