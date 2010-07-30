using System;
using System.Windows.Forms;
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.UI.Interop;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric
{ // UI page. 
  public partial class MakeMethodGenericPage : UserControl, IRefactoringPage
  {
    private readonly MakeMethodGenericWorkflow myWorkflow;
    private readonly IProperty<bool> myContinueEnabled = new Property<bool>("MakeMethodGenericPage", true);

    public MakeMethodGenericPage(MakeMethodGenericWorkflow workflow)
    {
      InitializeComponent();
      myWorkflow = workflow;
      myTextName.Text = workflow.TypeParameterName;
    }

    // 'Next' button is clicked. Commit data from from into workflow. 
    public IRefactoringPage Commit(IProgressIndicator pi)
    {
      myWorkflow.TypeParameterName = myTextName.Text;
      return null;
    }

    public bool DoNotShow
    {
      get { return false; }
    }

    /// <summary>
    /// Specific page may require long initialization. Run it here...
    /// </summary>
    public bool Initialize(IProgressIndicator progressIndicator)
    {
      return true;
    }

    /// <summary>
    /// This code is executed when documents were modified. data from workflow may be invalid. 
    /// </summary>
    public bool RefreshContents(IProgressIndicator progressIndicator)
    {
      if (!myWorkflow.IsValid())
        return false;
      return true;
    }

    public IProperty<bool> ContinueEnabled
    {
      get
      {
        return myContinueEnabled;
      }
    }

    public string Description
    {
      get
      {
        return "";
      }
    }

    public string Title
    {
      get
      {
        return "Specify name of type parameter";
      }
    }
    /// <summary>
    /// UI control itself
    /// </summary>
    public EitherControl View
    {
      get { return this; }
    }
  }
}