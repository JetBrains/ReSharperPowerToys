﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.PsiPlugin.CodeInspections;

namespace JetBrains.ReSharper.PsiPlugin.Inspection
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

    public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!IsSupported(process.SourceFile))
        return null;

      var factory = myServices.TryGetService<IPsiInspectionsProcessFactory>(process.SourceFile.LanguageType);
      if (factory == null)
        return null;

      return factory.CreateInspectionsProcess(process, settings);
    }
  }

  [ProjectFileType(typeof(KnownProjectFileType))]
  public class PsiInspectionsProcessFactory : IPsiInspectionsProcessFactory
  {
    public virtual IDaemonStageProcess CreateInspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings)
    {
      return new InspectionsProcess(process, settings);
    }
  }
}