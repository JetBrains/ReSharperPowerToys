namespace JetBrains.ReSharper.PowerToys.ZenCoding.Options.Model.Handlers
{
  public interface IPatternHandler
  {
    bool Matches(FileAssociation fileAssociation, string fileName);
  }
}