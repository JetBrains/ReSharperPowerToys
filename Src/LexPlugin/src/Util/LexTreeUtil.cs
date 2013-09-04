using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.LexPlugin.Grammar;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Parsing;
using JetBrains.ReSharper.LexPlugin.Psi.Lex.Tree;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.LexPlugin.Util
{
  internal static class LexTreeUtil
  {
    public static void ReplaceChild(ITreeNode parent, ITreeNode nameNode, string name)
    {
      if (name.IsEmpty())
      {
        throw new ArgumentException("name shouldn't be empty", "name");
      }

      using (WriteLockCookie.Create(parent.IsPhysical()))
      {
        ITreeNode identifier = LexElementFactory.GetInstance(parent.GetPsiModule()).CreateIdentifierExpression(name);
        LowLevelModificationUtil.ReplaceChildRange(nameNode, nameNode, identifier);
      }
    }
  }

  internal class LexElementFactory
  {
    private IPsiModule myModule;
    private LexLanguageService myLanguageService;

    public LexElementFactory([NotNull] IPsiModule module)
      : this(module, module.GetSolution())
    {
    }

    private LexElementFactory([NotNull] IPsiModule module, [NotNull] ISolution solution)
    {
      myModule = module;
      Solution = solution;
      Solution.GetPsiServices();
      myLanguageService = (LexLanguageService)LexLanguage.Instance.LanguageService();
    }

    protected ISolution Solution { get; set; }

    public static LexElementFactory GetInstance(IPsiModule psiModule)
    {
      return new LexElementFactory(psiModule);
    }

    private LexParser CreateParser(string text)
    {
      return (LexParser)myLanguageService.CreateParser(myLanguageService.GetPrimaryLexerFactory().CreateLexer(new StringBuffer(text)), null, null);
    }

    public ITreeNode CreateIdentifierExpression(string name)
    {
      var expression = CreateExpression("$0", name);
      return expression;
    }

    private ITreeNode CreateExpression(string format, string name)
    {
      ITreeNode node = CreateParser(name).ParseLexFile(false) as ILexFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
      }
      SandBox.CreateSandBoxFor(node, myModule);

      var child = node;
      while(child.FirstChild != null)
      {
        child = child.FirstChild;
      }

      if(child is IIdentifier)
      {
        return child;
      }

      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'" + name, format));
    }
  }
}
