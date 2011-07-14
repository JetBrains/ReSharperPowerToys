using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.ExternalSources.Utils;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation
{
  [SolutionComponentInterface]
  [SolutionComponentImplementation]
  public class ReflectorSpecificLogger : ISolutionComponent
  {
    private readonly ISolution mySolution;

    public ReflectorSpecificLogger(ISolution solution)
    {
      mySolution = solution;
    }

    #region ISolutionComponent Members

    public void Dispose()
    {
    }

    public void Init()
    {
    }

    public void AfterSolutionOpened()
    {
    }

    public void BeforeSolutionClosed()
    {
    }

    #endregion

    public static ReflectorSpecificLogger GetInstance(ISolution solution)
    {
      return solution.GetComponent<ReflectorSpecificLogger>();
    }

    private void Log(string s, bool justInformation)
    {
      s = "ReflectorNavigator: " + s;

      OutputPanelLogger.Instance.Log(s);
    }

    [StringFormatMethod("format")]
    public void LogFailure(string format, params object[] args)
    {
      Log(string.Format(format, args), false);
    }

    [StringFormatMethod("format")]
    public void LogInformation(string format, params object[] args)
    {
      Log(string.Format(format, args), true);
    }
  }
}