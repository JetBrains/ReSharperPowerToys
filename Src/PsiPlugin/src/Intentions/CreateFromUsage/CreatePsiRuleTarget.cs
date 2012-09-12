using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Parsing;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Psi.Psi.Tree.Impl;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Intentions.CreateFromUsage
{
  public class CreatePsiRuleTarget : ICreationTarget
  {
    private readonly PsiRuleReference myReference;
    private readonly ITreeNode myElement;
    private readonly IRuleDeclaration myDeclaration;

    private readonly bool myHasBraceParameters;
    private IList<Pair<string, string>> myVariableParameters = new List<Pair<string,string>>();
    private const string UndefinedRuleName = "_rule_name";
    private const string UndefinedParameterName = "_parameter_name_";

    public CreatePsiRuleTarget(PsiRuleReference reference)
    {
      myReference = reference;
      myElement = reference.GetTreeNode();
      string name = reference.GetName();

      var node = myElement.NextSibling;
      while (node != null)
      {
        if (!node.IsWhitespaceToken())
        {
          if (!(node is IRuleParameters))
          {
            break;
          }
          var child = node.FirstChild;
          while (child != null)
          {
            if (!child.IsWhitespaceToken())
            {
              if (child is IRuleBraceParameters)
              {
                myHasBraceParameters = true;
              }
              if (child is IRuleBracketParameters)
              {
                CollectVariableParameters(child as IRuleBracketParameters);
              }
            }
            child = child.NextSibling;
          }
          break;
        }
        node = node.NextSibling;
      }


      myDeclaration = PsiElementFactory.GetInstance(myElement.GetPsiModule()).CreateRuleDeclaration(name, myHasBraceParameters, myVariableParameters);

      Anchor = myElement.GetContainingNode<IRuleDeclaration>();
    }

    private void CollectVariableParameters(IRuleBracketParameters ruleBracketParameters)
    {
      var sibling = ruleBracketParameters.FirstChild;
      IList<ITreeNode> parameters = new List<ITreeNode>();
      while (sibling != null)
      {
        if ((sibling is VariableName) || (sibling.GetTokenType() == PsiTokenType.NULL_KEYWORD))
        {
          parameters.Add(sibling);
        }
        sibling = sibling.NextSibling;
      }

      foreach (var parameter in parameters)
      {
        if (parameter is VariableName)
        {
          var variableName = parameter as VariableName;
          var declaredElement = variableName.Resolve().DeclaredElement;
          if (declaredElement != null)
          {
            var variableDeclaration = declaredElement as IVariableDeclaration;
            string typeName = UndefinedRuleName;
            if(variableDeclaration.Parent is SharpExpression)
            {
              sibling = variableDeclaration.NextSibling;
              while(sibling != null)
              {
                if(sibling is IRuleName)
                {
                  typeName = sibling.GetText();
                  break;
                }
                sibling = sibling.NextSibling;
              }
            }

            if(variableDeclaration.Parent is RuleBracketTypedParameters)
            {
              sibling = variableDeclaration.PrevSibling;
              while(sibling != null)
              {
                if(! sibling.IsWhitespaceToken())
                {
                  if(! (sibling is IRuleName))
                  {
                    break;
                  } else
                  {
                    typeName = sibling.GetText();
                    break;
                  }
                }
                sibling = sibling.PrevSibling;
              }
            }

            myVariableParameters.Add(new Pair<string, string>(variableName.GetText(), typeName));
          }
          else
          {
            myVariableParameters.Add(new Pair<string, string>(variableName.GetText(), UndefinedRuleName));
          }
        }
        if (parameter.GetTokenType() == PsiTokenType.NULL_KEYWORD)
        {
          myVariableParameters.Add(new Pair<string, string>(UndefinedParameterName, UndefinedRuleName));
        }
      }
    }

    public ITreeNode GetTargetDeclaration()
    {
      return myDeclaration.Parent;
    }

    public IRuleDeclaration Declaration
    {
      get { return myDeclaration; }
    }

    public IFile GetTargetDeclarationFile()
    {
      return myElement.GetContainingFile();
    }

    public IEnumerable<ITreeNode> GetPossibleTargetDeclarations()
    {
      yield return myDeclaration.Parent;
    }

    public PsiRuleReference Reference
    {
      get { return myReference; }
    }

    public ITreeNode Anchor { get; private set; }
  }
}
