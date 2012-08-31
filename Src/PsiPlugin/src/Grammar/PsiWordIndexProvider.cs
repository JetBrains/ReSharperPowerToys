using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl.Caches2.WordIndex;

namespace JetBrains.ReSharper.PsiPlugin.Grammar
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
      return WordIndexTokenizerUtil.IsLetterFast(ch) || ch == '_' || ch == '$';
    }

    public bool IsIdentifierSecondLetter(char ch)
    {
      return WordIndexTokenizerUtil.IsLetterOrDigitFast(ch) || ch == '_' || ch == '$';
    }

    #endregion
  }
}
