using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.PsiPlugin.Grammar;

namespace JetBrains.ReSharper.PsiPlugin.Services
{
  //[ProjectFileType(typeof(PsiProjectFileType))]
  public class PsiFileTypeSpecificOwnerUtil : DefaultFileTypeSpecificOwnerUtil
  {
    public PsiFileTypeSpecificOwnerUtil()
    {
    }

    public override bool CanContainSeveralClasses(Psi.IPsiSourceFile sourceFile)
    {
      return false;
    }
  }
}