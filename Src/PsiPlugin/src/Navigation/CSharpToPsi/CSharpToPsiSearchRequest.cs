using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
using JetBrains.ReSharper.Feature.Services.Navigation.Search.SearchRequests;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.Refactoring;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace JetBrains.ReSharper.PsiPlugin.Navigation.CSharpToPsi
{
  public class CSharpToPsiSearchRequest : SearchRequest
  {
    private readonly DeclaredElementEnvoy<IDeclaredElement> myTarget;
    private readonly ISolution mySolution;
    private string myCachedTitle;

    public CSharpToPsiSearchRequest(IDeclaredElement declaredElement)
    {
      if (declaredElement == null)
        throw new ArgumentNullException("declaredElement");
      Logger.Assert(declaredElement.IsValid(), "declaredElement should be valid");

      mySolution = declaredElement.GetPsiServices().Solution;


      var @class = declaredElement as IClass;
      if (@class != null)
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class));
      }

      var @method = declaredElement as IMethod;
      if (@method != null)
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForMethod(@method));
      }

      var @interface = declaredElement as IInterface;
      if (@interface != null)
      {
        myTarget = new DeclaredElementEnvoy<IDeclaredElement>(DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForInterface(@interface));
      }

      var @constructor = declaredElement as IConstructor;

      if(@constructor != null)
      {
        @class = @constructor.GetContainingType() as IClass;

        if(@class != null)
        {
          myTarget = new DeclaredElementEnvoy<IDeclaredElement>(DerivedDeclaredElementUtil.GetPrimaryDeclaredElementForClass(@class));
        }
      }
    }

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