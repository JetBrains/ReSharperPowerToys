using JetBrains.Annotations;

namespace JetBrains.ReSharper.Psi.Jam
{
  [LanguageDefinition(Name)]
  public class JamLanguage : KnownLanguage
  {
    public new const string Name = "JAM";

    [UsedImplicitly] public static JamLanguage Instance;

    protected JamLanguage() : this(Name) {}

    protected JamLanguage([NotNull] string name) : this(name, name) {}

    protected JamLanguage([NotNull] string name, [NotNull] string presentableName) : base(name, presentableName) {}
  }
}