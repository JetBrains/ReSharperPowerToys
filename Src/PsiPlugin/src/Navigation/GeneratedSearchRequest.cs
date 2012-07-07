using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.DataContext;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace JetBrains.ReSharper.PsiPlugin.Navigation
{
  public abstract class GeneratedSearchRequest : SearchRequest
  {
    protected DeclaredElementEnvoy<IDeclaredElement> myTarget;
    protected ISolution mySolution;
    private string myCachedTitle;

    protected override ITaskExecutor CreateTaskExecutor()
    {
      return SimpleTaskExecutor.Instance;
    }

    protected virtual string GetTitle(IDeclaredElement declaredElement)
    {
      string kind = DeclaredElementPresenter.Format(PresentationUtil.GetPresentationLanguage(declaredElement), DeclaredElementPresenter.KIND_PRESENTER, declaredElement);
      string name = DeclaredElementPresenter.Format(PresentationUtil.GetPresentationLanguage(declaredElement), DeclaredElementPresenter.NAME_PRESENTER, declaredElement);
      return string.Format("Declarations of {1} '{0}'", name, kind);
    }

    public override string Title
    {
      get
      {
        var declaredElement = myTarget.GetValidDeclaredElement();
        if (declaredElement == null)
          return myCachedTitle;

        myCachedTitle = GetTitle(declaredElement);
        return myCachedTitle;
      }
    }

    public DeclaredElementEnvoy<IDeclaredElement> Target
    {
      get { return myTarget; }
    }

    public override ISolution Solution
    {
      get { return mySolution; }
    }

    public override ICollection SearchTargets
    {
      get { return new IDeclaredElementEnvoy[] { myTarget }; }
    }

    public override ICollection<IOccurence> Search(IProgressIndicator progressIndicator)
    {
      var declaredElement = myTarget.GetValidDeclaredElement();
      if (declaredElement == null)
        return EmptyList<IOccurence>.InstanceList;

      var consumer = new SearchResultsConsumer();
      declaredElement.GetPsiServices().AsyncFinder.FindDeclarations(declaredElement, progressIndicator, consumer);
      var result = consumer.GetOccurences();

      return result;
    }
  }
}
