namespace JetBrains.ReSharper.Psi.Jam.Impl.Tree
{
  internal partial class JamFile
  {
    #region IJamFile Members

    public override PsiLanguageType Language
    {
      get { return JamLanguage.Instance; }
    }

    #endregion
  }
}