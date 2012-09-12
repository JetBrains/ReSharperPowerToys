using JetBrains.ReSharper.Feature.Services.CodeStructure;
using JetBrains.ReSharper.Psi.Css.Parsing;
using JetBrains.ReSharper.Psi.Jam.CodeStyle;
using JetBrains.ReSharper.Psi.Jam.Impl;
using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeStructure
{
  internal class JamGlobalVariableCodeElement : JamCodeStructureElementBase<IGlobalVariableDeclaration>
  {
    private readonly JamCodeElementAspect myJamCodeElementAspect;

    public JamGlobalVariableCodeElement(IGlobalVariableDeclaration declaration, CodeStructureElement root, PsiIconManager psiIconManager) : base(declaration, root, psiIconManager)
    {
      myJamCodeElementAspect = new JamCodeElementAspect(this, JamDeclaredElementType.GlobalVariable);
    }

    public override IFileStructureAspect GetFileStructureAspect()
    {
      return myJamCodeElementAspect;
    }

    public override IGotoFileMemberAspect GetGotoMemberAspect()
    {
      return myJamCodeElementAspect;
    }

    public override IMemberNavigationAspect GetMemberNavigationAspect()
    {
      return myJamCodeElementAspect;
    }

    class JamCodeElementAspect : JamCodeElementAspectBase<IGlobalVariableDeclaration>
    {
      public JamCodeElementAspect(JamGlobalVariableCodeElement element, JamDeclaredElementType elementType) : base(element, elementType, JamCodeStyleColors.GlobalVariable) {}
    }
  }
}