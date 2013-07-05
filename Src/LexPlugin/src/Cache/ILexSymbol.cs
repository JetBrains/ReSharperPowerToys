using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.LexPlugin.Cache
{
  /// <summary>
  ///   Customization point for pdi properties
  /// </summary>
  [CannotApplyEqualityOperator]
  public interface ILexSymbol
  {
    /// <summary>
    ///   Offset of symbol declaration in the source fileFullName tree
    /// </summary>
    int Offset { get; }

    /// <summary>
    ///   Owner PSI source fileFullName
    /// </summary>
    IPsiSourceFile SourceFile { get; }

    string Name { get; }
  }
}
