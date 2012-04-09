using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace JetBrains.ReSharper.PsiPlugin.CodeInspections
{
  public interface IPsiInspectionsProcessFactory
  {
    IDaemonStageProcess CreateInspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings);
  }

  [DaemonStage(StagesBefore = new[] { typeof(SmartResolverStage), typeof(PsiFileIndexStage) })]
  public class InspectionsStage : PsiDaemonStageBase
  {
    private readonly ProjectFileTypeServices myServices;

    public InspectionsStage(ProjectFileTypeServices services)
    {
      myServices = services;
    }

    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return IsSupported(sourceFile) ? ErrorStripeRequest.STRIPE_AND_ERRORS : ErrorStripeRequest.NONE;
    }

    public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;

      var factory = myServices.TryGetService<IPsiInspectionsProcessFactory>(process.SourceFile.LanguageType);
      if (factory == null)
        return null;

      return new List<IDaemonStageProcess>(){factory.CreateInspectionsProcess(process, settings)};
    }
  }

  [ProjectFileType(typeof(KnownProjectFileType))]
  public sealed class PsiInspectionsProcessFactory : IPsiInspectionsProcessFactory
  {
    public IDaemonStageProcess CreateInspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings)
    {
      return new InspectionsProcess(process, settings);
    }
  }
}
