﻿using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Plugins.Unity.Cg.Psi.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Plugins.Unity.Cg.Daemon.Stages
{
    [DaemonStage(StagesBefore = new[] { typeof(GlobalFileStructureCollectorStage) },
        StagesAfter = new [] { typeof(CollectUsagesStage)} )]
    public class CgPreprocessorHighlightingStage : CgDaemonStageBase
    {
        protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings,
            DaemonProcessKind processKind, ICgFile file)
        {
            return new CgPreprocessorHighligthingProcess(process, settings, file);
        }

        private class CgPreprocessorHighligthingProcess : CgDaemonStageProcessBase
        {
            public CgPreprocessorHighligthingProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore, ICgFile file)
            #if RIDER
                : base(daemonProcess, file)
            #else
                : base(daemonProcess, settingsStore, file)
            #endif
            {
            }

            public override void VisitDirectiveNode(IDirective directiveParam, IHighlightingConsumer context)
            {
                context.AddHighlighting(new CgHighlighting(CgHighlightingAttributeIds.KEYWORD, directiveParam.HeaderNode.GetDocumentRange()));
                context.AddHighlighting(new CgHighlighting(CgHighlightingAttributeIds.PREPPROCESSOR_LINE_CONTENT, directiveParam.ContentNode.GetDocumentRange()));

                base.VisitDirectiveNode(directiveParam, context);
            }
        }
    }
}