using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.PsiPlugin.Resolve;
using JetBrains.ReSharper.PsiPlugin.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Tree.Impl
{
  class PsiReference : IReference
  {
    private bool isResultCached = false;
    private ResolveResultWithInfo myResolveResult;
    private ITreeNode myTreeNode;
    private string myName;

    public PsiReference(ITreeNode node)
    {
      myTreeNode = node;
      myName = node.GetText();
    }

    public void PutData<T>(Key<T> key, T val) where T : class
    {
      throw new NotImplementedException();
    }

    public T GetData<T>(Key<T> key) where T : class
    {
      throw new NotImplementedException();
    }

    public IEnumerable<KeyValuePair<object, object>> EnumerateData()
    {
      throw new NotImplementedException();
    }

    public ITreeNode GetTreeNode()
    {
      return myTreeNode;
    }

    public string GetName()
    {
      return myName;
    }

    public IEnumerable<string> GetAllNames()
    {
      throw new NotImplementedException();
    }

    public ISymbolTable GetReferenceSymbolTable()
    {
      return GetReferenceSymbolTable(true);
    }

    public ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      PsiFile file = ((PsiCompositeElement)myTreeNode).getContainingFile();
      return file.FileSymbolTable;
    }

    public ResolveResultWithInfo Resolve()
    {
      if (!isResultCached)
      {
        //IDeclaredElement declaredElement = getContainingFile().getDeclaration(GetName());
        ISymbolTable table = GetReferenceSymbolTable();
        IList<DeclaredElementInstance> elements = new List<DeclaredElementInstance>();
        if (table != null)
        {
          IList<ISymbolInfo> infos = table.GetSymbolInfos(GetName());
          foreach (ISymbolInfo info in infos)
          {
            var element = new DeclaredElementInstance(info.GetDeclaredElement(), EmptySubstitution.INSTANCE);
            elements.Add(element);
          }
        }
        myResolveResult = new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements),
                                                    ResolveErrorType.OK);
        isResultCached = true;
        return myResolveResult;
      } else
      {
        return myResolveResult;
      }
    }

    public TreeTextRange GetTreeTextRange()
    {
      return new TreeTextRange(new TreeOffset(myTreeNode.GetNavigationRange().TextRange.StartOffset), myTreeNode.GetText().Length);
    }

    public IReference BindTo(IDeclaredElement element)
    {
      //throw new NotImplementedException();
      //ITreeNode node = (ITreeNode) element;
      //IReference reference = new PsiReference(node);
      //((RuleDeclaration)element).RuleName.setReference(this);
      //IList<DeclaredElementInstance> elements = new List<DeclaredElementInstance>();
      //elements.Add(element);
      myResolveResult = new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResult(element),
                                                    ResolveErrorType.OK);
      isResultCached = true;

      IRuleName ruleName = (IRuleName) GetTreeNode();
      if (ruleName.Parent != null)
      {
        PsiTreeUtil.ReplaceChild(ruleName, ruleName.FirstChild, element.ShortName);
      }
      return this;
    }

    public IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      //throw new NotImplementedException();
      return BindTo(element);
    }

    public IAccessContext GetAccessContext()
    {
      throw new NotImplementedException();
    }

    public bool IsValid()
    {
      return true;
    }

    public bool HasMultipleNames
    {
      get { return false; }
    }

    public ResolveResultWithInfo CurrentResolveResult
    {
      get { return Resolve(); }
      set { throw new NotImplementedException(); }
    }
  }
}
