using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl.Caches2.WordIndex;

namespace JetBrains.ReSharper.PsiPlugin.PsiGrammar
{
  public class PsiWordIndexProvider : IWordIndexLanguageProvider
  {
    #region IWordIndexLanguageProvider Members

    public bool CaseSensitiveIdentifiers
    {
      get { return true; }
    }

    public bool IsIdentifierFirstLetter(char ch)
    {
      return ch.IsLetterFast() || ch == '_' || ch == '$';
    }

    public bool IsIdentifierSecondLetter(char ch)
    {
      return ch.IsLetterOrDigitFast() || ch == '_' || ch == '$';
    }

    #endregion
  }
}
