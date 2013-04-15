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

using JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model;
using JetBrains.UI.Application;
using JetBrains.UI.CommonControls;
using JetBrains.UI.Components;

namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options
{
  public partial class EditFileAssociationControl : SafeUserControl
  {
    bool myUpdateCookie;

    public EditFileAssociationControl(UIApplication environment) : base(environment)
    {
    }

    public EditFileAssociationControl(FileAssociation fileAssociation, UIApplication environment) : base(environment)
    {
      InitializeComponent();

      SetUpValues(fileAssociation);
      SetUpValueChangedHandlers();
    }

    public FileAssociation FileAssociation { get; private set; }

    void SetUpValues(FileAssociation fileAssociation)
    {
      FileAssociation = fileAssociation;

      myPattern.Text = FileAssociation.Pattern;
      myEnabled.Checked = FileAssociation.Enabled;

      switch (FileAssociation.DocType)
      {
        case DocType.Html:
          myHtml.Checked = true;
          break;
        case DocType.Css:
          myCss.Checked = true;
          break;
        case DocType.Xsl:
          myXsl.Checked = true;
          break;
        case DocType.None:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      switch (FileAssociation.PatternType)
      {
        case PatternType.FileExtension:
          myFileExtension.Checked = true;
          break;
        case PatternType.Regex:
          myRegex.Checked = true;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    void SetUpValueChangedHandlers()
    {
      myPattern.TextChanged += ParamsChanged;

      myFileExtension.CheckedChanged += ParamsChanged;
      myRegex.CheckedChanged += ParamsChanged;

      myHtml.CheckedChanged += ParamsChanged;
      myCss.CheckedChanged += ParamsChanged;
      myXsl.CheckedChanged += ParamsChanged;

      myEnabled.CheckedChanged += ParamsChanged;
    }

    void ParamsChanged(object sender, EventArgs e)
    {
      if (!myUpdateCookie)
      {
        myUpdateCookie = true;

        FileAssociation.Pattern = myPattern.Text;

        if (myFileExtension.Checked)
        {
          FileAssociation.PatternType = PatternType.FileExtension;
        }

        if (myRegex.Checked)
        {
          FileAssociation.PatternType = PatternType.Regex;
        }

        if (myHtml.Checked)
        {
          FileAssociation.DocType = DocType.Html;
        }

        if (myCss.Checked)
        {
          FileAssociation.DocType = DocType.Css;
        }

        if (myXsl.Checked)
        {
          FileAssociation.DocType = DocType.Xsl;
        }

        FileAssociation.Enabled = myEnabled.Checked;

        myUpdateCookie = false;
      }
    }
  }
}