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

using System.Runtime.Serialization;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  [DataContract]
  public class GistFile
  {
    [DataMember(Name = "size")]
    public int Size { get; set; }
    [DataMember(Name = "filename")]
    public string Filename { get; set; }
    [DataMember(Name = "raw_url")]
    public string RawUrl { get; set; }
    [DataMember(Name = "content")]
    public string Content { get; set; }
  }
}