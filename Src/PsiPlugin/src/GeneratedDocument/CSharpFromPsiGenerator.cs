using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Impl.Shared;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.Psi.Web.Generation;

namespace JetBrains.ReSharper.PsiPlugin.GeneratedDocument
{
  class CSharpFromPsiGenerator
  {
    private IPsiFile myFile;
    private int myCurrentTextLength;
    private GenerationResults myGeneratedMethodBody;
    private GenerationResults myGeneratedFile;

    public CSharpFromPsiGenerator(IPsiFile file)
    {
      myFile = file;
    }

    public GenerationResults Generate()
    {
      myGeneratedMethodBody = new GenerationResults(CSharpLanguage.Instance, "", GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile));
      myGeneratedFile = new GenerationResults(CSharpLanguage.Instance, "", GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile));

      addOptions(myFile);

      myGeneratedFile.Append(new GenerationResults(CSharpLanguage.Instance, "class A{\n void foo(){\n", GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile)));
      myGeneratedFile.Append(myGeneratedMethodBody);
      myGeneratedFile.Append(new GenerationResults(CSharpLanguage.Instance, "}\n }\n", GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile)));

      return myGeneratedFile;
    }

    private void addOptions(ITreeNode treeNode)
    {
      if( treeNode is IOptionsDefinition || treeNode is IInterfacesDefinition || treeNode is IRuleDeclaration || treeNode is IPsiFile)
      {
        var child = treeNode.FirstChild;
        while(child != null)
        {
          addOptions(child);
          child = child.NextSibling;
        }
      }
      if(treeNode is IOptionDefinition)
      {
        var value = (treeNode as IOptionDefinition).OptionStringValue;
        int startOffset;
        int endOffset;
        if(value != null)
        {
          string optionName = (treeNode as IOptionDefinition).OptionName.GetText();
          if((optionName.Length > 0) && ("\"".Equals(optionName.Substring(0,1))))
          {
            optionName = optionName.Substring(1, optionName.Length - 1);
          }
          if ((optionName.Length > 0) && ("\"".Equals(optionName.Substring(optionName.Length - 1, 1))))
          {
            optionName = optionName.Substring(0, optionName.Length - 1);
          }

          string optionValueText = value.GetText();
          startOffset = value.GetTreeStartOffset().Offset;
          if((optionValueText.Length > 0) && ("\"".Equals(optionValueText.Substring(0,1))))
          {
            optionValueText = optionValueText.Substring(1, optionValueText.Length - 1);
            startOffset++;
          }
          endOffset = value.GetTreeEndOffset().Offset;
          if ((optionValueText.Length > 0) && ("\"".Equals(optionValueText.Substring(optionValueText.Length - 1, 1))))
          {
            optionValueText = optionValueText.Substring(0, optionValueText.Length - 1);
            endOffset--;
          }

          if(optionValueText.Length > 0)
          {
            if(OptionDeclaredElements.NamespacesOptions.Contains(optionName))
            {
              var map = GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile);
              map.Add(new TreeTextRange<Generated>(new TreeOffset(6), new TreeOffset(6 + optionValueText.Length)),
                      new TreeTextRange<Original>(new TreeOffset(startOffset), new TreeOffset(endOffset)));
              myGeneratedFile.Append(new GenerationResults(CSharpLanguage.Instance,"using " + optionValueText + ";\n", map));              
            }
            if (OptionDeclaredElements.ClassesOptions.Contains(optionName))
            {
              var classes = treeNode.GetPsiServices().CacheManager.GetDeclarationsCache(treeNode.GetPsiModule(), false, true).GetTypeElementsByCLRName(optionValueText);
              foreach (var typeElement in classes)
              {
                if(typeElement is IClass)
                {
                  if(((IClass) typeElement).IsStatic)
                  {
                    var fields = ((IClass) typeElement).Fields;
                    foreach (var field in fields)
                    {
                      var staticMap = GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile);
                      staticMap.Add(new TreeTextRange<Generated>(new TreeOffset(), new TreeOffset(optionValueText.Length)),
                              new TreeTextRange<Original>(new TreeOffset(startOffset), new TreeOffset(endOffset)));
                      myGeneratedMethodBody.Append(new GenerationResults(CSharpLanguage.Instance, optionValueText + "." + field.ShortName + " a;\n",staticMap));
                      return;
                    }
                  }
                }
              }
              var map = GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile);
              map.Add(new TreeTextRange<Generated>(new TreeOffset(), new TreeOffset(optionValueText.Length)),
                      new TreeTextRange<Original>(new TreeOffset(startOffset), new TreeOffset(endOffset)));
              myGeneratedMethodBody.Append(new GenerationResults(CSharpLanguage.Instance, optionValueText + " a;\n", map));
            }
            if(OptionDeclaredElements.MethodsOptions.Contains(optionName))
            {
              var map = GeneratedRangeMapFactory.CreateGeneratedRangeMap(myFile);
              map.Add(new TreeTextRange<Generated>(new TreeOffset(), new TreeOffset(optionValueText.Length)),
                      new TreeTextRange<Original>(new TreeOffset(startOffset), new TreeOffset(endOffset)));
              myGeneratedMethodBody.Append(new GenerationResults(CSharpLanguage.Instance,optionValueText + "\n", map));           
            }
          }
        }
      }
    }
  }
}
