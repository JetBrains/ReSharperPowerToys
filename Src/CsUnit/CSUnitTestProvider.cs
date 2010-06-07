using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.CommonControls;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.UI;
using JetBrains.TreeModels;
using JetBrains.UI;
using JetBrains.UI.TreeView;
using JetBrains.Util;

namespace JetBrains.ReSharper.PowerToys.CsUnit
{
  [UnitTestProvider]
  internal class CSUnitTestProvider : IUnitTestProvider
  {
    internal const string CSUnit_ID = "CSUnit";
    private static readonly CLRTypeName TestFixtureAttribute = new CLRTypeName("csUnit.TestFixtureAttribute");
    private static readonly CLRTypeName TestAttribute = new CLRTypeName("csUnit.TestAttribute");
    private static readonly CLRTypeName IgnoreAttribute = new CLRTypeName("csUnit.IgnoreAttribute");
    private static readonly CLRTypeName SetUpAttribute = new CLRTypeName("csUnit.SetUpAttribute");
    private static readonly CLRTypeName TearDownAttribute = new CLRTypeName("csUnit.TearDownAttribute");

    private static readonly CSUnitTestPresenter ourPresenter = new CSUnitTestPresenter();

    public string ID
    {
      get { return CSUnit_ID; }
    }

    public string Name
    {
      get { return CSUnit_ID; }
    }

    public Image Icon
    {
      get { return ImageLoader.GetImage("csunit"); }
    }

    public ProviderCustomOptionsControl GetCustomOptionsControl(ISolution solution)
    {
      return null;
    }

    private static bool IsTestFixture(ITypeElement typeElement)
    {
      if (!(typeElement is IClass) && !(typeElement is IStruct))
        return false;
      
      if (typeElement.GetAllTypeParameters().Any())
        return false; // type should be concrete

      var modifiersOwner = (IModifiersOwner)typeElement;
      if (modifiersOwner.IsAbstract || modifiersOwner.GetAccessRights() == AccessRights.INTERNAL)
        return false;

      if (typeElement.HasAttributeInstance(TestFixtureAttribute, false))
        return true;

      var @class = typeElement as IClass;
      if (@class != null)
      {
        var visited = new HashSet<IClass>();
        while ((@class = @class.GetSuperClass()) != null)
        {
          if (visited.Contains(@class))
            break;
          visited.Add(@class);
          if (@class.HasAttributeInstance(TestFixtureAttribute, false))
            return true;
        }
      }

      return false;
    }

    private static bool IsTestMethod(ITypeMember element)
    {
      var method = element as IMethod;
      if (method == null)
        return false;

      if (method.IsStatic || method.IsAbstract ||
          method.GetAccessRights() != AccessRights.PUBLIC ||
          method.Parameters.Count > 0 || !method.ReturnType.IsVoid())
        return false;

      if (method.HasAttributeInstance(TestAttribute, false))
        return true;

      if (method.ShortName.ToLower(CultureInfo.InvariantCulture).StartsWith("test"))
      {
        foreach (IAttributeInstance instance in method.GetAttributeInstances(false))
        {
          string clrName = instance.CLRName;
          if (clrName != null && clrName.StartsWith("csUnit."))
            return false;
        }

        return true;
      }

      return false;
    }

    private static bool IsTestFixture(IMetadataTypeInfo typeInfo)
    {
      if (!typeInfo.IsAbstract && (typeInfo.IsPublic || typeInfo.IsNestedPublic) && typeInfo.GenericParameters.Length == 0)
        return HasTestFixtureAttribute(typeInfo);
      return false;
    }

    private static bool HasTestFixtureAttribute(IMetadataTypeInfo typeInfo)
    {
      if (typeInfo.HasCustomAttribute(TestFixtureAttribute.ClrName))
        return true;

      IMetadataClassType baseType = typeInfo.Base;
      if (baseType != null)
        return HasTestFixtureAttribute(baseType.Type);

      return false;
    }

    private static bool IsTestMethod(IMetadataMethod method)
    {
      if (method.IsStatic || method.IsAbstract || !method.IsPublic) return false;
      if (method.Parameters.Length != 0 || method.GenericArguments.Length != 0) return false;
      if (!method.ReturnValue.Type.AssemblyQualifiedName.StartsWith("System.Void")) return false;

      if (method.HasCustomAttribute(TestAttribute.ClrName))
        return true;

      if (method.Name.StartsWith("test", true, CultureInfo.InvariantCulture))
      {
        IMetadataCustomAttribute[] attributes = method.CustomAttributes;
        foreach (IMetadataCustomAttribute attribute in attributes)
        {
          string name = GetAttributeName(attribute);
          if (name != null && name.StartsWith("csUnit."))
            return false;
        }
        return true;
      }

      return false;
    }




    private static string GetAttributeName(IMetadataCustomAttribute attribute)
    {
      IMetadataMethod constructor = attribute.UsedConstructor;
      if (constructor == null)
        return null;

      IMetadataTypeInfo declaringType = constructor.DeclaringType;
      if (declaringType == null)
        return null;

      return declaringType.FullyQualifiedName;
    }


    private static string GetExplicitString(IMetadataEntity entity)
    {
      IList<IMetadataCustomAttribute> attributes = entity.GetCustomAttributes(IgnoreAttribute.ClrName);
      string reason = null;
      for (int i = 0; i < attributes.Count; i++)
      {
        object[] arguments = attributes[i].ConstructorArguments;
        if (arguments.Length == 1)
        {
          var r = arguments[0] as string;
          if (r == null)
          {
            if (reason == null)
              reason = "";
          }
          else
          {
            reason = r;
          }
        }
        else
        {
          if (reason == null)
            reason = "";
        }
      }
      return reason;
    }

    public void ExploreAssembly(IMetadataAssembly assembly, IProject project, UnitTestElementConsumer consumer)
    {
      foreach (IMetadataTypeInfo type in assembly.GetTypes())
      {
        if (!IsTestFixture(type))
          continue;


        var fixture = new CSUnitTestFixtureElement(this, project, type.FullyQualifiedName, assembly.Location);
        fixture.SetExplicit(GetExplicitString(type));

        consumer(fixture);
        int order = 0;
        foreach (IMetadataMethod method in GetAllTestMethods(type))
        {
          if (!IsTestMethod(method))
            continue;

          var testElement = new CSUnitTestElement(this, fixture, project, method.DeclaringType.FullyQualifiedName, method.Name, order++);
          testElement.SetExplicit(GetExplicitString(method));
          consumer(testElement);
        }
      }
    }

    private static List<IMetadataMethod> GetAllTestMethods(IMetadataTypeInfo typeInfo)
    {
      var list = new List<IMetadataMethod>();
      var map = new OneToListMap<string, IMetadataMethod>();

      while (typeInfo != null)
      {
        foreach (IMetadataMethod method in typeInfo.GetMethods())
        {
          if (!IsTestMethod(method)) continue;

          if (map.ContainsKey(method.Name) && (method.IsVirtual))
          {
            bool hasOverride = false;
            foreach (IMetadataMethod metadataMethod in map[method.Name])
            {
              if (metadataMethod.IsVirtual && !metadataMethod.IsNewSlot)
                hasOverride = true;
            }

            if (hasOverride)
              continue;
          }

          map.AddValue(method.Name, method);
          list.Add(method);
        }

        IMetadataClassType baseType = typeInfo.Base;
        typeInfo = (baseType != null) ? baseType.Type : null;
      }

      return list;
    }

    public void ExploreFile(IFile psiFile, UnitTestElementLocationConsumer consumer, CheckForInterrupt interrupted)
    {
      if (psiFile == null)
        throw new ArgumentNullException("psiFile");

      psiFile.ProcessDescendants(new MyFileExplorer(this, consumer, psiFile, interrupted));
    }

    private static bool IsUnitTestStuff(IDeclaredElement element)
    {
      if (IsUnitTest(element) || IsUnitTestContainer(element))
        return true;

      var method = element as IMethod;
      if (method != null)
      {
        if (method.HasAttributeInstance(SetUpAttribute, false)) return true;
        if (method.HasAttributeInstance(TearDownAttribute, false)) return true;
      }
      return false;
    }

    private static bool IsUnitTest(IDeclaredElement element)
    {
      var typeMember = element as ITypeMember;
      if (typeMember == null)
        return false;

      var typeElement = typeMember.GetContainingType();
      return typeElement != null && IsTestFixture(typeElement) && IsTestMethod(typeMember);
    }

    private static bool IsUnitTestContainer(IDeclaredElement element)
    {
      var typeElement = element as ITypeElement;
      return typeElement != null && IsTestFixture(typeElement);
    }

    public bool IsElementOfKind(IDeclaredElement declaredElement, UnitTestElementKind elementKind)
    {
      switch (elementKind)
      {
        case UnitTestElementKind.Unknown:
          return !IsUnitTestStuff(declaredElement);
        case UnitTestElementKind.Test:
          return IsUnitTest(declaredElement);
        case UnitTestElementKind.TestContainer:
          return IsUnitTestContainer(declaredElement);
        case UnitTestElementKind.TestStuff:
          return IsUnitTestStuff(declaredElement);
        default:
          throw new ArgumentOutOfRangeException("elementKind");
      }
    }

    public bool IsElementOfKind(UnitTestElement element, UnitTestElementKind elementKind)
    {
      switch (elementKind)
      {
        case UnitTestElementKind.Unknown:
          return !(element is CSUnitElementBase);
        case UnitTestElementKind.Test:
          return element is CSUnitTestElement;
        case UnitTestElementKind.TestContainer:
          return element is CSUnitTestFixtureElement;
        case UnitTestElementKind.TestStuff:
          return element is CSUnitElementBase;
        default:
          throw new ArgumentOutOfRangeException("elementKind");
      }
    }

    private class MyFileExplorer : IRecursiveElementProcessor
    {
      private readonly IUnitTestProvider myProvider;
      private readonly UnitTestElementLocationConsumer myConsumer;
      private readonly IFile myFile;
      private readonly CheckForInterrupt myInterrupted;
      private readonly IProject myProject;
      private readonly Dictionary<IDeclaredElement, CSUnitTestFixtureElement> myFixtures = new Dictionary<IDeclaredElement, CSUnitTestFixtureElement>();
      private readonly Dictionary<IDeclaredElement, int> myOrders = new Dictionary<IDeclaredElement, int>();
      private readonly string myAssemblyPath;
      
      public MyFileExplorer(IUnitTestProvider provider, UnitTestElementLocationConsumer consumer, IFile file, CheckForInterrupt interrupted)
      {
        if (file == null)
          throw new ArgumentNullException("file");
        if (provider == null)
          throw new ArgumentNullException("provider");

        myConsumer = consumer;
        myProvider = provider;
        myFile = file;
        myInterrupted = interrupted;
        myProject = myFile.ProjectFile.GetProject();
        myAssemblyPath = UnitTestManager.GetOutputAssemblyPath(myProject).FullPath;
      }

      public bool InteriorShouldBeProcessed(IElement element)
      {
        return !(element is ITypeMemberDeclaration) || (element is ITypeDeclaration);
      }

      public bool ProcessingIsFinished
      {
        get {
          if (myInterrupted())
            throw new ProcessCancelledException();
          return false;
        }
      }

      public void ProcessBeforeInterior(IElement element)
      {
        var declaration = element as IDeclaration;
        if (declaration == null)
          return;
        CSUnitElementBase testElement = null;

        var declaredElement = declaration.DeclaredElement;
        var typeElement = declaredElement as ITypeElement;
        if (typeElement != null && IsTestFixture(typeElement))
        {
          CSUnitTestFixtureElement fixtureElement;

          if (!myFixtures.ContainsKey(typeElement))
          {
            fixtureElement = new CSUnitTestFixtureElement(myProvider, myProject, typeElement.CLRName, myAssemblyPath);
            myFixtures.Add(typeElement, fixtureElement);
            myOrders.Add(typeElement, 0);
          }
          else
          {
            fixtureElement = myFixtures[typeElement];
          }

          testElement = fixtureElement;
          int order = 0;
          AppendTests(fixtureElement, typeElement.GetSuperTypes(), ref order);
        }


        var typeMember = declaredElement as ITypeMember;
        if (typeMember != null && IsTestMethod(typeMember))
        {
          ITypeElement type = typeMember.GetContainingType();
          if (type != null)
          {
            CSUnitTestFixtureElement fixtureElement = myFixtures.TryGetValue(type);
            if (fixtureElement != null)
            {
              int order = myOrders.TryGetValue(type) + 1;
              myOrders[type] = order;
              testElement = new CSUnitTestElement(myProvider, fixtureElement, myProject, type.CLRName, typeMember.ShortName, order);
            }
          }
        }

        if (testElement == null)
          return;
        var disposition = new UnitTestElementDisposition(testElement, myFile.ProjectFile, declaration.GetNameDocumentRange().TextRange, declaration.GetDocumentRange().TextRange);
        myConsumer(disposition);
      }

      private void AppendTests(CSUnitTestFixtureElement fixtureElement, IEnumerable<IDeclaredType> types, ref int order)
      {
        foreach (IDeclaredType type in types)
        {
          ITypeElement typeElement = type.GetTypeElement();
          if (typeElement == null)
            continue;
          foreach (ITypeMember member in typeElement.GetMembers())
          {
            if (IsTestMethod(member))
              new CSUnitTestElement(myProvider, fixtureElement, myProject, typeElement.CLRName, member.ShortName, order++);
          }
          AppendTests(fixtureElement, type.GetSuperTypes(), ref order);
        }
      }

      public void ProcessAfterInterior(IElement element)
      {
      }
    }

    public void ExploreSolution(ISolution solution, UnitTestElementConsumer consumer)
    {
    }

    public void ExploreExternal(UnitTestElementConsumer consumer)
    {
    }

    public void Present(UnitTestElement element, IPresentableItem presentableItem, TreeModelNode node, PresentationState state)
    {
      ourPresenter.UpdateItem(element, node, presentableItem, state);
    }

    public RemoteTaskRunnerInfo GetTaskRunnerInfo()
    {
      return new RemoteTaskRunnerInfo(typeof (CSUnitTaskRunner));
    }

    public string Serialize(UnitTestElement element)
    {
      return null;
    }

    public UnitTestElement Deserialize(ISolution solution, string elementString)
    {
      return null;
    }

    public IList<UnitTestTask> GetTaskSequence(UnitTestElement element, IList<UnitTestElement> explicitElements)
    {
      var testElement = element as CSUnitTestElement;
      if (testElement != null)
      {
        CSUnitTestFixtureElement parentFixture = testElement.Fixture;
        return new[]
                 {
                   new UnitTestTask(null, new AssemblyLoadTask(parentFixture.AssemblyLocation)),
                   new UnitTestTask(parentFixture, new CSUnitTestFixtureTask(parentFixture.AssemblyLocation, parentFixture.GetTypeClrName(), explicitElements.Contains(parentFixture))),
                   new UnitTestTask(testElement, new CSUnitTestTask(parentFixture.GetTypeClrName(), testElement.MethodName, explicitElements.Contains(testElement))),
                 };
      }
      var fixture = element as CSUnitTestFixtureElement;
      if (fixture != null)
        return EmptyArray<UnitTestTask>.Instance;

      throw new ArgumentException(string.Format("element is not CSUnit: '{0}'", element));
    }

    public int CompareUnitTestElements(UnitTestElement x, UnitTestElement y)
    {
      if (Equals(x, y))
        return 0;
      
      int compare = StringComparer.CurrentCultureIgnoreCase.Compare(x.GetTypeClrName(), y.GetTypeClrName());
      if (compare != 0)
        return compare;
      if (x is CSUnitTestElement && y is CSUnitTestFixtureElement)
        return -1;
      if (y is CSUnitTestElement && x is CSUnitTestFixtureElement)
        return 1;
      if (x is CSUnitTestFixtureElement && y is CSUnitTestFixtureElement)
        return 0; // two different elements with same type name??

      var xe = (CSUnitTestElement)x;
      var ye = (CSUnitTestElement)y;
      return xe.Order.CompareTo(ye.Order);
    }
  }
}