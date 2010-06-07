using System.Drawing;
using JetBrains.CommonControls;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.ReSharper.Psi;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  /// <summary>
  /// Provides information for TreeModelBrowser about what and how to display
  /// </summary>
  internal class TypeInterfaceDescriptor : TreeModelBrowserDescriptorPsi
  {
    // Hide static and protected members for root, if true
    private readonly bool myInstanceOnly;
    
    // Root type element envoy
    private DeclaredElementEnvoy<ITypeElement> myTypeElementEnvoy;
    
    // Presentation provider
    private readonly TreeModelBrowserPresenter myPresenter;

    // Cached title
    private string myTitle;

    // Model
    private TypeInterfaceModel myModel;

    public TypeInterfaceDescriptor(ITypeElement typeElement, bool instanceOnly) : base(typeElement.GetManager().Solution)
    {
      AutoExpandSingleChild = true;
      myInstanceOnly = instanceOnly;
      
      // We use standard presenter, but emphasize root element using adorements
      myPresenter = new TypeInterfacePresenter
                      {
                        DrawElementExtensions = true,
                        ShowOccurenceCount = false,
                        PostfixTypeQualification = true
                      };
      myPresenter.PresentAdorements += PresentAdorements;

      // Wrap typeElement with an envoy, so it can survive code changes
      myTypeElementEnvoy = new DeclaredElementEnvoy<ITypeElement>(typeElement);
      MakeModel();
    }


    public ITypeElement TypeElement
    {
      get { return myTypeElementEnvoy.GetValidDeclaredElement(); }
      set
      {
        if (Equals(myTypeElementEnvoy.GetValidDeclaredElement(), value))
          return;
        myTypeElementEnvoy = new DeclaredElementEnvoy<ITypeElement>(value);
        MakeModel();
      }
    }

    public override string Title
    {
      get { return myTitle; }
    }

    public override TreeModel Model
    {
      get { return myModel; }
    }

    public override StructuredPresenter<TreeModelNode, IPresentableItem> Presenter
    {
      get { return myPresenter; }
    }

    private void MakeModel()
    {
      UpdateTitle();
      // Create new model with recursion prevention
      var model = new TypeInterfaceModel(myTypeElementEnvoy, myInstanceOnly)
                    {
                      RecursionPrevention = RecursionPreventionStyle.StopOnOccurence
                    };
      myModel = model;
      // Use our comparer, which sorts by member kind first
      myModel.Comparer = DelegatingComparer<TreeModelNode, object>.Create(source => source.DataValue, new TypeInterfaceModelComparer());
      
      // Descriptor is finished configuring itself, so request updating visual representation, i.e. tree view
      RequestUpdate(UpdateKind.Structure, true);
    }

    private void UpdateTitle()
    {
      // do not update title if element was lost
      if (TypeElement == null)
        return;

      myTitle = FormatTypeElement("Type '{0}'");
    }

    private string FormatTypeElement(string format)
    {
      // uses DeclaredElementPresenter to format type element, which is standard way to present code elements
      var style = new DeclaredElementPresenterStyle(NameStyle.SHORT)
                    {
                      ShowTypeParameters = TypeParameterStyle.FULL
                    };
      string typeElementText = DeclaredElementPresenter.Format(PresentationUtil.GetPresentationLanguage(TypeElement), style, TypeElement);
      return string.Format(format, typeElementText);
    }

    private void PresentAdorements(object value, IPresentableItem item, TreeModelNode structureElement, PresentationState state)
    {
      // Emphasize root element
      var element = value as IDeclaredElement;
      if (element == null)
      {
        var envoy = value as DeclaredElementEnvoy<ITypeElement>;
        if (envoy != null)
          element = envoy.GetValidDeclaredElement();
      }
      if (Equals(element, TypeElement))
        item.RichText.SetStyle(FontStyle.Bold);

      // Recursion was stopped, i.e. same type member appeared higher in the chain
/*
      if ((modelNode.Modifiers & TreeModelNodeModifiers.Recursive) != TreeModelNodeModifiers.None)
        item.Images.Add(ourRecursionImage, "Recursive inheritance", ImagePlacement.RIGHT);
*/
    }
  }
}