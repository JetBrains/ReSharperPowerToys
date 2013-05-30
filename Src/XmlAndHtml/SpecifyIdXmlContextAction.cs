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
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Xml.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Xml.ContextActions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Parsing;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace XmlAndHtml
{
  /// <summary>
  /// This context action works on XML files. When you're on a tag that doesn't have an
  /// 'id' attribute, the context action creates it.
  /// </summary>
  [ContextAction(
    Group = "XML",
    Name = "Specify Id",
    Description = "Creates an 'id' attribute for the selected tag of an XML document",
    Priority = 0)]
  public class SpecifyIdXmlContextAction : ContextActionBase
  {
    private readonly XmlContextActionDataProvider myDataProvider;

    public SpecifyIdXmlContextAction(XmlContextActionDataProvider dataProvider)
    {
      myDataProvider = dataProvider;
    }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
      IXmlTagHeader tagHeader = GetTagHeader();
      if (tagHeader == null)
        return null;

      IXmlTag tag = XmlTagNavigator.GetByTagHeader(tagHeader);
      if (tag == null)
        return null;

      var factory = XmlElementFactory<XmlLanguage>.GetByNodeLanguage(tagHeader);

      IXmlAttribute idAttr = factory.CreateAttributeForTag(tag, "id=\"\"");

      tag.AddAttributeBefore(idAttr, null);

      // continuation to do after transaction commited
      return textControl =>
        // move cursor inside new created id attribute
        textControl.Caret.MoveTo(idAttr.Value.GetDocumentRange().TextRange.StartOffset, CaretVisualPlacement.Generic);
    }

    /// <summary>
    /// Returns the test that is rendered on the context action.
    /// </summary>
    public override string Text
    {
      get { return "Specify 'id'"; }
    }

    public override bool IsAvailable(IUserDataHolder dataHolder)
    {
      // grab the tag we're on
      IXmlTagHeader tagHeader = GetTagHeader();
      if (tagHeader == null)
        return false;

      // check if the attribute is already there (case-insensitive)
      IXmlAttribute idAtt = tagHeader.GetAttribute(attr => StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeName, "id"));
      if (idAtt != null)
        return false;

      return true;
    }

    private IXmlTagHeader GetTagHeader() { return myDataProvider.FindNodeAtCaret<IXmlTagHeader>(); }
  }
}
