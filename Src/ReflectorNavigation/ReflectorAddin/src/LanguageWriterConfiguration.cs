using Reflector.CodeModel;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public class LanguageWriterConfiguration : ILanguageWriterConfiguration
  {
    private readonly IVisibilityConfiguration myVisibility = new VisibilityConfiguration();

    #region ILanguageWriterConfiguration Members

    public IVisibilityConfiguration Visibility
    {
      get { return myVisibility; }
    }

    public string this[string name]
    {
      get
      {
        // TODO ?
        switch (name)
        {
          case "ShowDocumentation":
          case "ShowCustomAttributes":
          case "ShowNamespaceImports":
          case "ShowNamespaceBody":
          case "ShowTypeDeclarationBody":
          case "ShowMethodDeclarationBody":
            return "true";
        }

        return "false";
      }
    }

    #endregion

    #region Nested type: VisibilityConfiguration

    private class VisibilityConfiguration : IVisibilityConfiguration
    {
      #region IVisibilityConfiguration Members

      public bool Public
      {
        get { return true; }
      }

      public bool Private
      {
        get { return true; }
      }

      public bool Family
      {
        get { return true; }
      }

      public bool Assembly
      {
        get { return true; }
      }

      public bool FamilyAndAssembly
      {
        get { return true; }
      }

      public bool FamilyOrAssembly
      {
        get { return true; }
      }

      #endregion
    }

    #endregion
  }
}