using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.LexPlugin.Grammar
{
  [LanguageDefinition(Name)]
  public class LexLanguage : KnownLanguage
  {
    private new const string Name = "Lex";

    [UsedImplicitly]
    public static LexLanguage Instance;

    protected LexLanguage()
      : base(Name, Name)
    {
    }

    protected LexLanguage([NotNull] string name)
      : base(name, name)
    {
    }

    protected LexLanguage([NotNull] string name, [NotNull] string presentableName)
      : base(name, presentableName)
    {
    }
  }
}
