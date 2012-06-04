/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Html.Bulbs;
using JetBrains.ReSharper.Intentions;
using JetBrains.ReSharper.Intentions.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.Html.Parsing;
using JetBrains.ReSharper.Psi.Html.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using System.Linq;
using JetBrains.ReSharper.Psi.Tree;

namespace XmlAndHtml
{
  [ContextAction(Group = "HTML", Name = "Specify Id", Description = "Creates an 'id' attribute for the selected tag of an HTML document")]
  public class SpecifyIdHtmlContextAction : BulbItemImpl, IContextAction
  {
    private readonly IWebContextActionDataProvider<IHtmlFile> myProvider;

    public SpecifyIdHtmlContextAction(IWebContextActionDataProvider<IHtmlFile> provider)
    {
      myProvider = provider;
    }

    public void CreateBulbItems(BulbMenu menu)
    {
      menu.ArrangeContextAction(this);
    }

    public bool IsAvailable(IUserDataHolder cache)
    {
      IHtmlTagHeader tagHeader = GetTag();
      if (tagHeader == null)
        return false;

      // check if the attribute is already there (case-insensitive)
      return !tagHeader.Attributes.Any(a => a.AttributeName.Equals("id", StringComparison.OrdinalIgnoreCase));
    }

    private IHtmlTagHeader GetTag()
    {
      return myProvider.FindNodeAtCaret<IHtmlTagHeader>();
    }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      IHtmlTagHeader tagHeader = GetTag();
      if (tagHeader == null)
        return null;

      // The easiest way to create an attribute is to create an HTML tag with an attribute in it
      // and then get the attribute from the tag.
      HtmlElementFactory factory = HtmlElementFactory.GetInstance(tagHeader.Language);
      IHtmlTag dummy = factory.CreateHtmlTag("<tag id=\"\"/>", tagHeader);
      ITagAttribute idAttr = dummy.Attributes.Single();
      tagHeader.AddAttributeBefore(idAttr, null);

      // continuation to do after transaction commited
      return textControl => 
        // move cursor inside new created id attribute
        textControl.Caret.MoveTo(idAttr.ValueElement.GetDocumentRange().TextRange.StartOffset, CaretVisualPlacement.Generic);
    }

    public override string Text
    {
      get { return "Specify 'id'"; }
    }
  }
}