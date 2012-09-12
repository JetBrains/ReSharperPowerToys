using System;
using System.Collections.Generic;
using JetBrains.CommonControls;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeStructure;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Jam.Impl;
using JetBrains.ReSharper.Psi.Jam.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TreeModels;
using JetBrains.UI.PopupMenu;
using JetBrains.UI.RichText;
using JetBrains.UI.TreeView;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Jam.CodeStructure
{
  internal abstract class JamCodeElementAspectBase<T> : IFileStructureAspect, IGotoFileMemberAspect, IMemberNavigationAspect where T : class, IJamDeclaration
  {
    private static readonly DeclaredElementPresenterStyle myPresenterStyle = new DeclaredElementPresenterStyle(NameStyle.SHORT) {ShowParameterNames = true, ShowParameterContainer = ParameterContainerStyle.AFTER_IN_PARENTHESIS};

    private readonly JamCodeStructureElementBase<T> myElement;
    private readonly JamDeclaredElementType myElementType;
    private readonly TextStyle myTextStyle;

    protected JamCodeElementAspectBase(JamCodeStructureElementBase<T> element, JamDeclaredElementType elementType, TextStyle textStyle)
    {
      myElement = element;
      myElementType = elementType;
      myTextStyle = textStyle;
    }

    #region Test

    public DocumentRange NavigationRange
    {
      get
      {
        var psiElement = myElement.PsiElement;
        return psiElement == null ? DocumentRange.InvalidRange : psiElement.GetNameDocumentRange();
      }
    }

    #endregion

    public bool InitiallyExpanded
    {
      get { return false; }
    }

    public void Present(StructuredPresenter<TreeModelNode, IPresentableItem> presenter, IPresentableItem item, TreeModelNode modelNode, PresentationState state)
    {
      item.Images.Add(myElement.PsiIconManager.GetImage(myElementType));
      item.RichText = GetPresentation();
    }

    public void Present(IMenuItemDescriptor descriptor, PresentationState state)
    {
      descriptor.Icon = myElement.PsiIconManager.GetImage(myElementType);
      descriptor.Text = GetPresentation();
    }

    private RichText GetPresentation()
    {
      var text = new RichText(SharedImplUtil.MISSING_DECLARATION_NAME, myTextStyle);

      var psiElement = myElement.PsiElement;
      if (psiElement != null)
      {
        var declaredElement = psiElement.DeclaredElement;
        if (declaredElement != null)
        {
          DeclaredElementPresenterMarking marking;
          var name = DeclaredElementPresenter.Format(JamLanguage.Instance, myPresenterStyle, declaredElement, out marking);

          text = new RichText(name);
          text.SetStyle(myTextStyle, marking.NameRange.StartOffset, marking.NameRange.Length);
        }
      }

      return text;
    }

    public DocumentRange[] GetNavigationRanges()
    {
      return new [] { NavigationRange };
    }

    public IList<string> GetQuickSearchTexts()
    {
      var psiElement = myElement.PsiElement;
      return psiElement != null ? new List<string> {psiElement.DeclaredName} : EmptyList<string>.InstanceList;
    }

    public bool CanRemove()
    {
      return false;
    }

    public void Remove()
    {
      throw new NotImplementedException();
    }

    public bool CanRename()
    {
      return false;
    }

    public string InitialName()
    {
      throw new NotImplementedException();
    }

    public void Rename(string newName)
    {
      throw new NotImplementedException();
    }

    public bool CanMoveElements(RelativeLocation location, IList<CodeStructureElement> dropElements)
    {
      // TODO : implemenet it
      return false;
    }

    public void MoveElements(RelativeLocation location, IList<CodeStructureElement> dropElements)
    {
      // TODO : implemenet it
      throw new NotImplementedException();
    }
  }
}