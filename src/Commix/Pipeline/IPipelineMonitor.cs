using System;

using Commix.Core.Diagnostics;

namespace Commix.Pipeline
{
    public interface IPipelineMonitor
    {
        void OnCompleteEvent(PipelineEventArgs e);
        void OnRunEvent(PipelineEventArgs e);
        void OnErrorEvent(PipelineErrorEventArgs e);
        void OnProcessorRunEvent(PipelineProcessorEventArgs e);
        void OnProcessorCompleteEvent(PipelineProcessorEventArgs e);
        void OnProcessorExceptionEvent(PipelineProcessorExceptionEventArgs e);
    }
}