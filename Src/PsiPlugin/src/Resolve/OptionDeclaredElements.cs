using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  class OptionDeclaredElements
  {
    public static IList<string> FileOptionNames = new List<string>()
                                                   {
                                                     "parserClassName",
                                                     "parserPackage",
                                                     "psiInterfacePackageName",
                                                     "tokenTypePackageName",
                                                     "psiStubsPackageName",
                                                     "psiStubsBaseClass",
                                                     "tokenTypeBeforeFirst",
                                                     "tokenTypeFirst",
                                                     "tokenTypeLast",
                                                     "elementTypeBeforeFirst",
                                                     "tokenClassName",
                                                     "tokenTypePrefix",
                                                     "elementTypePrefix",
                                                     "visitorClassName",
                                                     "visitorMethodSuffix",
                                                     "visitorSuperClassName",
                                                     "customImplPackage",
                                                     "customInterfacePackage",
                                                     "defaultErrorHandling",
                                                     "disableReflection",
                                                     "parserTargetSubdir",
                                                     "tokenTypeTargetSubdir",
                                                     "psiInterfacesTargetSubdir",
                                                     "psiStubsTargetSubdir",
                                                     "testTargetSubdir",
                                                     "semanticVisitorClassName",
                                                     "hierarchicalSemanticVisitorClassName",
                                                     "separateHierarchies",
                                                     "tokenBitsetThreshold",
                                                     "suppressVisitor",
                                                     "languageType",
                                                     "publicChildRolePrefix",
                                                     "publicChildRoleClass",
                                                     "generateWorkingPsi",
                                                     "parserMessagesClass"
                                                   };

    public static IList<string> RuleOptionNames = new List<string>()
                                                   {
                                                     "customImpl",
                                                     "customInterface",
                                                     "customVisit",
                                                     "customNavigator",
                                                     "customConstructor",
                                                     "customToString",
                                                     "interfaceName",
                                                     "elementType",
                                                     "elementTypeSet",
                                                     "stubBase",
                                                     "fakeStub",
                                                     "result",
                                                     "multilevel",
                                                     "interfaceBaseName",
                                                     "customFollows",
                                                     "customParseFunction",
                                                     "noInternalParseFunction",
                                                     "hasSemanticInterface",
                                                     "noInterface",
                                                     "parsingContext",
                                                     "expectedSymbol"
                                                   };
  }
}
