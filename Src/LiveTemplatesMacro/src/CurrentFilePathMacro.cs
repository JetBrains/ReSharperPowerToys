/*
 * Copyright 2007-2014 JetBrains
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

using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.LiveTemplatesMacro
{
  [MacroDefinition("LiveTemplatesMacro.CurrentFilePath", // macro name should be unique among all other macros, it's recommended to prefix it with your plugin name to achieve that
    ShortDescription = "Current file path", // description of the macro to be shown in the list of macros
    LongDescription="Evaluates current file path" // long description of the macro to be shown in the area below the list
    )]
  public class CurrentFilePathMacro : IMacroDefinition
  {
    public string GetPlaceholder(IDocument document, IEnumerable<IMacroParameterValue> parameters)
    {
      return "a";
    }

    public ParameterInfo[] Parameters
    {
      get
      {
        return EmptyArray<ParameterInfo>.Instance;
      }
    }
  }
}