using JetBrains.ReSharper.Feature.Services.CodeStructure;
using JetBrains.ReSharper.Psi.Jam.CodeStyle;
using JetBrains.ReSharper.Psi.Jam.Impl;
using JetBrains.ReSharper.Psi.Jam.Tree;

namespace JetBrains.ReSharper.Psi.Jam.CodeStructure
{
  internal class JamProcedureCodeElement : JamCodeStructureElementBase<IProcedureDeclaration>
  {
    private readonly JamCodeElementAspect myJamCodeElementAspect;

    public JamProcedureCodeElement(IProcedureDeclaration declaration, CodeStructureElement root, PsiIconManager psiIconManager) : base(declaration, root, psiIconManager)
    {
      myJamCodeElementAspect = new JamCodeElementAspect(this, JamDeclaredElementType.Procedure);
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

    class JamCodeElementAspect : JamCodeElementAspectBase<IProcedureDeclaration>
    {
      public JamCodeElementAspect(JamProcedureCodeElement element, JamDeclaredElementType elementType) : base(element, elementType, JamCodeStyleColors.ProcedureName) {}
    }
  }
}