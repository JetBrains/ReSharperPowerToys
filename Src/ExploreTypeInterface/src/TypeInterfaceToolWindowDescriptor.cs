/*
 * Copyright 2007-2011 JetBrains
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

using JetBrains.UI.Application;
using JetBrains.UI.ToolWindowManagement;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  [ToolWindowDescriptor(
    Text = "Type Interface",
    ProductNeutralId = "2EBEA6DE-578A-4234-A782-54F4F09B61D5",
    VisibilityPersistenceScope = ToolWindowVisibilityPersistenceScope.Solution,
    Type = ToolWindowType.MultiInstance)]
    public class TypeInterfaceToolWindowDescriptor : ToolWindowDescriptor {
    public TypeInterfaceToolWindowDescriptor(IWindowBranding branding)
      : base(branding)
    {
    }
  }
}