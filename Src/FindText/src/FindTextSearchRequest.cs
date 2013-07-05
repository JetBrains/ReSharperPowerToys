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

using System.Collections;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation.Occurences;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.FindText
{
  /// <summary>
  /// Implements SearchRequest which finds specified string in solution
  /// </summary>
  public class FindTextSearchRequest : SearchRequest
  {
    // Solution to search in
    // Case sensitivity flag

    private readonly bool _caseSensitive;
    private readonly string _searchString;
    private readonly FindTextSearchFlags _searchFlags;
    private readonly ISolution _solution;

    public FindTextSearchRequest(ISolution solution, string searchString, bool caseSensitive, FindTextSearchFlags searchFlags)
    {
      _solution = solution;
      _searchFlags = searchFlags;
      _searchString = searchString;
      _caseSensitive = caseSensitive;
    }

    public override ICollection<IOccurence> Search(IProgressIndicator progressIndicator)
    {
      // Create and configure utility class, which will perform search
      var searcher = new StringSearcher(_searchString, _caseSensitive);

      // Fetch all projects for solution, start progress 
      ICollection<IProject> projects = Solution.GetAllProjects();
      progressIndicator.Start(projects.Count);
      var items = new List<IOccurence>();

      // visit each project and collect occurences
      var visitor = new ProjectTextSearcher(searcher, items, _searchFlags, _solution);
      foreach (IProject project in projects)
      {
        progressIndicator.CurrentItemText = string.Format("Scanning project '{0}'", project.Name);
        project.Accept(visitor);
        progressIndicator.Advance(1);
      }

      return items;
    }

    public override ICollection SearchTargets
    {
      get { return new[] {string.Format("Text \"{0}\"", _searchString)}; }
    }

    public override ISolution Solution
    {
      get { return _solution; }
    }

    public override string Title
    {
      get { return string.Format("Text '{0}'", _searchString); }
    }

    #region ProjectTextSearcher Type

    /// <summary>
    /// Class which visits project files recursively in the project and performs search
    /// </summary>
    private class ProjectTextSearcher : RecursiveProjectVisitor
    {
      private readonly ISolution _solution;
      private readonly List<IOccurence> _items;
      private readonly StringSearcher _searcher;
      private readonly FindTextSearchFlags _searchFlags;

      public ProjectTextSearcher(StringSearcher searcher, List<IOccurence> items, FindTextSearchFlags searchFlags, ISolution solution)
      {
        _searcher = searcher;
        _searchFlags = searchFlags;
        _items = items;
        _solution = solution;
      }

      public override void VisitProjectFile(IProjectFile projectFile)
      {
        base.VisitProjectFile(projectFile);
        var sourceFile = projectFile.ToSourceFile();
        if (sourceFile == null)
          return;

        using (ReadLockCookie.Create())
        {
          // Obtain document for visited file and find all text occurences
          var document = sourceFile.Document;

          // Obtain lexer for projectFile if needed
          ILexer lexer = null;
          if (_searchFlags != FindTextSearchFlags.All)
          {
            // Content should be provided to the following call, because sometimes lexer depends on content
            // E.g. ASP with C# or VB script language
            IBuffer contentBuffer = document.Buffer;
            var factory = PsiProjectFileTypeCoordinator.Instance;
            var lexerFactory = factory.CreateLexerFactory(_solution, projectFile.LanguageType, contentBuffer, sourceFile);
            if (lexerFactory != null)
            {
              lexer = lexerFactory.CreateLexer(contentBuffer);
              lexer.Start();
            }
          }

          foreach (int offset in _searcher.FindAll(document.Buffer))
          {
            // create TextualOccurence for each found text and add to collection
            var textRange = new TextRange(offset, offset + _searcher.Pattern.Length);

            if (lexer != null)
            {
              // Fastforward lexer to found location
              while (lexer.TokenEnd < offset)
                lexer.Advance();
            }

            if (lexer != null && lexer.TokenType != null)
            {
              var tokentype = lexer.TokenType;
              if ((_searchFlags & FindTextSearchFlags.StringLiterals) == FindTextSearchFlags.None &&
                  tokentype.IsStringLiteral)
                continue;
              if ((_searchFlags & FindTextSearchFlags.Comments) == FindTextSearchFlags.None && tokentype.IsComment)
                continue;
              if ((_searchFlags & FindTextSearchFlags.Other) == FindTextSearchFlags.None &&
                  (!tokentype.IsComment && !tokentype.IsStringLiteral))
                continue;
            }
            else
            {
              if ((_searchFlags & FindTextSearchFlags.Other) == FindTextSearchFlags.None)
                continue;
            }           
            var options = new OccurencePresentationOptions();
            _items.Add(new RangeOccurence(sourceFile, new DocumentRange(document, textRange), OccurenceType.TextualOccurence, options));
          }
        }
      }
    }

    #endregion
  }
}