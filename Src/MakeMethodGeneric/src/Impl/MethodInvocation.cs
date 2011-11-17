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

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.PowerToys.MakeMethodGeneric.Impl
{
  public class MethodInvocation
  {
    public MethodInvocation(IReference reference, IType type, IMethod method, ISubstitution substitution)
    {
      Reference = reference;
      Type = type;
      Method = method;
      Substitution = substitution;
    }

    public IReference Reference { get; private set; }
    public IType Type { get; private set; }
    public IMethod Method { get; private set; }
    public ISubstitution Substitution { get; private set; }

    public bool IsValid()
    {
      return Reference.IsValid() && Type.IsValid() && Method.IsValid() && Substitution.IsValid();
    }
  }
}