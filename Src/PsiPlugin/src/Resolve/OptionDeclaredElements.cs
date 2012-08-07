using System.Collections.Generic;

namespace JetBrains.ReSharper.PsiPlugin.Resolve
{
  public static class OptionDeclaredElements
  {
    public static readonly IList<string> FileOptionNames = new List<string>
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
                                                             "publicChildRolePrefix",
                                                             "publicChildRoleClass",
                                                             "generateWorkingPsi",
                                                             "parserMessagesClass",
                                                           };

    public static readonly IList<string> NamespacesOptions = new List<string>
                                                             {
                                                               "parserPackage",
                                                               "psiInterfacePackageName",
                                                               "tokenTypePackageName",
                                                               "psiStubsPackageName",
                                                               "parserGenRuntimePackageName",
                                                               "customImplPackage",
                                                               "customInterfacePackage"
                                                             };

    public static readonly IList<string> ClassesOptions = new List<string>
                                                          {
                                                            "parserClassName",
                                                            "psiStubsBaseClass",
                                                            "tokenTypeBeforeFirst",
                                                            "tokenTypeFirst",
                                                            "tokenTypeLast",
                                                            "elementTypeBeforeFirst",
                                                            "tokenClassName",
                                                            "semanticVisitorClassName",
                                                            "hierarchicalSemanticVisitorClassName",
                                                            "visitorClassName",
                                                            "visitorSuperClassName",
                                                            "treeElementClassFQName",
                                                            "leafElementClassFQName",
                                                            "compositeElementClassFQName",
                                                            "psiElementVisitorClassFQName",
                                                            "tokenTypeClassFQName",
                                                            "unexpectedTokenClassFQName",
                                                            "syntaxErrorClassFQName",
                                                            "lexerClassName",
                                                            "psiElementClassFQName",
                                                            "semanticPsiElementClassFQName",
                                                            "tokenElementClassFQName",
                                                            "objectClassFQName",
                                                            "parserMessagesClass",
                                                            "tokenBitsetThreshold",
                                                            "elementTypeBaseClass"
                                                          };

    public static readonly IList<string> MethodsOptions = new List<string>
                                                          {
                                                            "createTokenElementMethodFQName",
                                                          };

    public static readonly IList<string> DirectoryOptions = new List<string>
                                                            {
                                                              "parserTargetSubdir",
                                                              "psiInterfacesTargetSubdir",
                                                              "psiStubsTargetSubdir",
                                                              "testTargetSubdir"
                                                            };

    public static readonly IList<string> RuleOptionNames = new List<string>
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
