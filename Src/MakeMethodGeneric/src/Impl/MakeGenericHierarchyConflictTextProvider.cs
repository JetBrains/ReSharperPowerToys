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

using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl
{
  public class MakeGenericHierarchyConflictTextProvider : HierarchyConflictTextProviderBase
  {
    public override string WillAlsoOverride()
    {
      return "Converted {0} also overrides {1} that will not be generified. Please resolve conflict manually.";
    }

    public override string WillAlsoImplement()
    {
      return "Converted {0} also implements {1} that will not be generified. Please resolve conflict manually.";
    }

    public override string QuasiImplements()
    {
      return "Converted {0} is quasi implemented by {1} that will not be generified. Please resolve conflict manually.";
    }
  }
}