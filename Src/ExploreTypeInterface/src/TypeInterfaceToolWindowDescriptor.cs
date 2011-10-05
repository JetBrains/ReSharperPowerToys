using JetBrains.UI.ToolWindowManagement;

namespace JetBrains.ReSharper.PowerToys.ExploreTypeInterface
{
  [ToolWindowDescriptor(
    Id = "TypeInterface",
    Text = "Type Interface",
    Guid = "2EBEA6DE-578A-4234-A782-54F4F09B61D5",
    VisibilityPersistenceScope = ToolWindowVisibilityPersistenceScope.Solution,
    Type = ToolWindowType.MultiInstance)]
  public class TypeInterfaceToolWindowDescriptor : ToolWindowDescriptor { }
}