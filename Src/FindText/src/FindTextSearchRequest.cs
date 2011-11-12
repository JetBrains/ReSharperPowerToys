using System.Collections;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
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

    private readonly bool myCaseSensitive;
    private readonly string mySearchString;
    private readonly FindTextSearchFlags mySearchFlags;
    private readonly DocumentManager myDocumentManager;
    private readonly ISolution mySolution;

    public FindTextSearchRequest(ISolution solution, string searchString, bool caseSensitive,
                                 FindTextSearchFlags searchFlags, DocumentManager documentManager)
    {
      mySolution = solution;
      mySearchFlags = searchFlags;
      myDocumentManager = documentManager;
      mySearchString = searchString;
      myCaseSensitive = caseSensitive;
    }

    public override ICollection<IOccurence> Search(IProgressIndicator progressIndicator)
    {
      // Create and configure utility class, which will perform search
      var searcher = new StringSearcher(mySearchString, myCaseSensitive);

      // Fetch all projects for solution, start progress 
      ICollection<IProject> projects = Solution.GetAllProjects();
      progressIndicator.Start(projects.Count);
      var items = new List<IOccurence>();

      // visit each project and collect occurences
      var visitor = new ProjectTextSearcher(searcher, items, mySearchFlags, myDocumentManager, mySolution);
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
      get { return new[] {string.Format("Text \"{0}\"", mySearchString)}; }
    }

    public override ISolution Solution
    {
      get { return mySolution; }
    }

    public override string Title
    {
      get { return string.Format("Text '{0}'", mySearchString); }
    }

    #region ProjectTextSearcher Type

    /// <summary>
    /// Class which visits project files recursively in the project and performs search
    /// </summary>
    private class ProjectTextSearcher : RecursiveProjectVisitor
    {
      private readonly DocumentManager myDocumentManager;
      private readonly ISolution mySolution;

      private readonly List<IOccurence> myItems;

      private readonly StringSearcher mySearcher;

      private readonly FindTextSearchFlags mySearchFlags;

      public ProjectTextSearcher(StringSearcher searcher, List<IOccurence> items, FindTextSearchFlags searchFlags, DocumentManager documentManager, ISolution solution)
      {
        mySearcher = searcher;
        mySearchFlags = searchFlags;
        myItems = items;
        myDocumentManager = documentManager;
        mySolution = solution;
      }

      public override void VisitProjectFile(IProjectFile projectFile)
      {
        base.VisitProjectFile(projectFile);
        using (ReadLockCookie.Create())
        {
          // Obtain document for visited project file and find all text occurences
          IDocument document = myDocumentManager.GetOrCreateDocument(projectFile);

          // Obtain lexer for projectFile if needed
          ILexer lexer = null;
          if (mySearchFlags != FindTextSearchFlags.All)
          {
            // Content should be provided to the following call, because sometimes lexer depends on content
            // E.g. ASP with C# or VB script language
            IBuffer contentBuffer = document.Buffer;
            var factory = PsiProjectFileTypeCoordinator.Instance;
            var lexerFactory = factory.CreateLexerFactory(mySolution, projectFile.LanguageType, contentBuffer, projectFile.ToSourceFile());
            if (lexerFactory != null)
            {
              lexer = lexerFactory.CreateLexer(contentBuffer);
              lexer.Start();
            }
          }

          foreach (int offset in mySearcher.FindAll(document.Buffer))
          {
            // create TextualOccurence for each found text and add to collection
            var textRange = new TextRange(offset, offset + mySearcher.Pattern.Length);

            if (lexer != null)
            {
              // Fastforward lexer to found location
              while (lexer.TokenEnd < offset)
                lexer.Advance();
            }

            if (lexer != null && lexer.TokenType != null)
            {
              var tokentype = lexer.TokenType;
              if ((mySearchFlags & FindTextSearchFlags.StringLiterals) == FindTextSearchFlags.None &&
                  tokentype.IsStringLiteral)
                continue;
              if ((mySearchFlags & FindTextSearchFlags.Comments) == FindTextSearchFlags.None && tokentype.IsComment)
                continue;
              if ((mySearchFlags & FindTextSearchFlags.Other) == FindTextSearchFlags.None &&
                  (!tokentype.IsComment && !tokentype.IsStringLiteral))
                continue;
            }
            else
            {
              if ((mySearchFlags & FindTextSearchFlags.Other) == FindTextSearchFlags.None)
                continue;
            }
            var options = new OccurencePresentationOptions();
            myItems.Add(new EmptyTextualOccurence(projectFile, textRange, options));
          }
        }
      }
    }

    #endregion
  }
}