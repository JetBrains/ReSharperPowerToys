﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Naming.Interfaces;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.DaemonStage;
using JetBrains.ReSharper.PsiPlugin.Feature.Services;
using JetBrains.ReSharper.PsiPlugin.Grammar;
using JetBrains.ReSharper.PsiPlugin.Parsing;
using JetBrains.ReSharper.PsiPlugin.Tree;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Inspection
{
  internal class InspectionsProcess : PsiDaemonStageProcessBase
  {
    //private readonly Stack<IFunctionExpression> myFunctions = new Stack<IFunctionExpression>();
    private readonly PsiFileIndexProcess myFileIndex;
    private readonly bool myIsStandaloneScript;
    //private IPsiFile myFile;
    private IDictionary<string, List<IDeclaration>> myDeclarations;
    //private readonly INamingPolicyProvider myPolicyProvider;

    public InspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings)
      : base(process, settings)
    {
      myIsStandaloneScript = process.SourceFile.PrimaryPsiLanguage.Is<PsiLanguage>();
      myFileIndex = process.GetStageProcess<PsiFileIndexProcess>();
      //myFile = process.SourceFile.GetPsiFile(PsiLanguage.Instance) as IPsiFile;
      myDeclarations = new Dictionary<string, List<IDeclaration>>();
      VisitFile(process.SourceFile.GetPsiFile(PsiLanguage.Instance) as IPsiFile);
      //myPolicyProvider = PsiServices.Naming.Policy.GetPolicyProvider(PsiLanguage.Instance, process.SourceFile, settings);
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
    }

    public override void VisitNode(ITreeNode element, IHighlightingConsumer consumer)
    {
      var ruleName = element as IRuleName;
      if (ruleName != null)
      {
        string name = ruleName.GetText();
        if (myDeclarations.ContainsKey(name))
        {
          var list = myDeclarations.GetValue(name);
          if (list.Count > 1)
          {
            consumer.AddHighlighting(new DuplicatingLocalDeclarationWarning(ruleName), File);
          }
        }
      }

      var ruleDeclaration = element as IRuleDeclaration;
      if(ruleDeclaration != null)
      {
        IRuleBody body = ruleDeclaration.Body;
        var child = PsiTreeUtil.GetFirstChild<IRuleName>(body);
        /*while ((child != null) && !(child is IRuleName))
        {
          child = child.NextSibling;
        }*/
        ruleName = child as IRuleName;
        if (ruleName != null)
        {
          if (ruleName.GetText().Equals(ruleDeclaration.DeclaredName))
          {
            consumer.AddHighlighting(new LeftRecursionWarning(ruleName), File);
          }
        }
      }

      var psiExpression = element as IPsiExpression;
      if(psiExpression != null)
      {
        var child = psiExpression.FirstChild;
        IList<ISequence> list = new List<ISequence>();
        while ( child != null)
        {
          if(child is ISequence)
          {
            list.Add(child as ISequence);
          }
          if( child is IChoiceTail)
          {
            list.Add((child as IChoiceTail).Sequence);
          }
          child = child.NextSibling;
        }

        if(list.Count > 1)
        {
          var sequences = list.ToArray();
          bool[] isRepeated = new bool[sequences.Count()];
          for (int i = 0; i < sequences.Count() - 1; ++i)
          {
            if (!isRepeated[i])
            {
              var sequence1 = sequences[i];
              for (int j = i + 1; j < sequences.Count(); ++j)
              {
                var sequence2 = sequences[j];
                if(PsiTreeUtil.EqualsElements(sequence1, sequence2))
                {
                  if (!isRepeated[i])
                  {
                    consumer.AddHighlighting(new RepeatedChoiceWarning(sequence1), File);
                    isRepeated[i] = true;
                  }
                  consumer.AddHighlighting(new RepeatedChoiceWarning(sequence2), File);
                  isRepeated[j] = true;
                }
              }
            }
          }
        }
      }
      base.VisitNode(element, consumer);
    }

    private void VisitFile(IPsiFile element)
    {
      var child = element.FirstChild;
      while (child != null)
      {
        if (child is IRuleDeclaration)
        {
          var declaration = child as IRuleDeclaration;
          if (declaration != null){
            string name = declaration.DeclaredName;
            if (myDeclarations.ContainsKey(name))
            {
              var list = myDeclarations.GetValue(name);
              list.Add(declaration);
            }
            else
            {
              var list = new List<IDeclaration>();
              list.Add(declaration);
              myDeclarations.Add(name, list);
            }
          }
        }
        child = child.NextSibling;
      }

      child = element.Interfaces;
      if (child != null)
      {
        child = child.FirstChild;
        while (child != null)
        {
          var declaration = child as IRuleDeclaration;
          if (declaration != null)
          {
            string name = declaration.DeclaredName;
            if (myDeclarations.ContainsKey(name))
            {
              var list = myDeclarations.GetValue(name);
              list.Add(declaration);
            }
            else
            {
              var list = new List<IDeclaration>();
              list.Add(declaration);
              myDeclarations.Add(name, list);
            }
          }
          child = child.NextSibling;
        }
      }
    }
  }
}
