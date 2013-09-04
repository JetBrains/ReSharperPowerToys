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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JetBrains.ReSharper.PowerToys.Gist.GitHub
{
  [DataContract]
  public class Gist
  {
    [DataMember(Name = "html_url")]
    public string HtmlUrl { get; set; }
    [DataMember(Name = "url")]
    public string Url { get; set; }
    [DataMember(Name = "id")]
    public long Id { get; set; }
    [DataMember(Name = "user")]
    public User User { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "public")]
    public bool IsPublic { get; set; }
    [DataMember(Name = "files")]
    public Dictionary<string, GistFile> Files { get; set; }
    [DataMember(Name = "comments")]
    public int Comments { get; set; }
    [DataMember(Name = "git_pull_url")]
    public string GitPullUrl { get; set; }
    [DataMember(Name = "git_push_url")]
    public string GitPushUrl { get; set; }
    [DataMember(Name = "created_at")]
    public DateTime CreatedAt { get; set; }
  }
}