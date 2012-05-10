using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Grammar
{
  [LanguageDefinition(Name)]
  public class PsiLanguage : KnownLanguage
  {
    private new const string Name = "PSI";

    [UsedImplicitly]
    public static PsiLanguage Instance;

    protected PsiLanguage()
      : base(Name, Name)
    {
    }

    protected PsiLanguage([NotNull] string name)
      : base(name, name)
    {
    }

    protected PsiLanguage([NotNull] string name, [NotNull] string presentableName)
      : base(name, presentableName)
    {
    }
  }
}
