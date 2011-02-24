using System.Collections.Generic;
using Reflector;
using Reflector.CodeModel;
using Reflector.CodeModel.Memory;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public class CodeGenerator
  {
    private readonly ILanguageManager myLanguageManager;
    private readonly ITranslatorManager myTranslatorManager;

    public CodeGenerator(ILanguageManager languageManager, ITranslatorManager translatorManager)
    {
      myLanguageManager = languageManager;
      myTranslatorManager = translatorManager;
    }

    public string Decompile(ITypeDeclaration typeDeclaration, string ext, bool xmlDoc)
    {
      var languageName = Ext2LanguageName(ext);
      if (languageName == null)
        return "";

      ILanguage language = GetLanguage(languageName);
      ILanguageWriterConfiguration configuration = new LanguageWriterConfiguration();
      var formatter = new TextFormatter();
      ILanguageWriter writer = language.GetWriter(formatter, configuration);

      ITranslator translator = myTranslatorManager.CreateDisassembler(xmlDoc ? "Xml" : null, null);
      typeDeclaration = translator.TranslateTypeDeclaration(typeDeclaration, true, true);

      if (!string.IsNullOrEmpty(typeDeclaration.Namespace))
      {
        INamespace namespaceItem = new Namespace {Name = typeDeclaration.Namespace};
        namespaceItem.Types.Add(typeDeclaration);
        writer.WriteNamespace(namespaceItem);
      }
      else
        writer.WriteTypeDeclaration(typeDeclaration);

      return formatter.ToString();
    }

    private static string Ext2LanguageName(string ext)
    {
      switch (ext.ToLowerInvariant())
      {
        case "cs":
          return "C#";
        case "vb":
          return "Visual Basic";
        default:
          return null;
      }
    }

    private ILanguage GetLanguage(string name)
    {
      foreach (ILanguage language in myLanguageManager.Languages)
        if (language.Name == name)
          return language;

      throw new KeyNotFoundException("Can't find " + name + " in language manager");
    }
  }
}