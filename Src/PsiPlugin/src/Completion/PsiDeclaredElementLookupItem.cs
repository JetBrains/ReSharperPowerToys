using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.Completion
{
  public class PsiDeclaredElementLookupItem : DeclaredElementLookupItemImpl
  {
    public PsiDeclaredElementLookupItem([NotNull] DeclaredElementInstance instance,
                                        [NotNull] IElementPointerFactory elementPointerFactory,
                                        [NotNull] PsiLanguageType languageType, ILookupItemsOwner owner)
      : base(instance, elementPointerFactory, languageType, owner)
    {
    }
  }
}
