using System;

using Commix.Diagnostics;

namespace Commix.Pipeline
{
    public interface IPipelineMonitor
    {
        event EventHandler<PipelineEventArgs> RunEvent;
        event EventHandler<PipelineEventArgs> CompleteEvent;
        event EventHandler<PipelineErrorEventArgs> ErrorEvent;
        event EventHandler<PipelineProcessorEventArgs> ProcessorRunEvent;
        event EventHandler<PipelineProcessorEventArgs> ProcessorCompleteEvent;
        event EventHandler<PipelineProcessorExceptionEventArgs> ProcessorExceptionEvent;

        void OnCompleteEvent(PipelineEventArgs e);
        void OnRunEvent(PipelineEventArgs e);
        void OnErrorEvent(PipelineErrorEventArgs e);
        void OnProcessorRunEvent(PipelineProcessorEventArgs e);
        void OnProcessorCompleteEvent(PipelineProcessorEventArgs e);
        void OnProcessorExceptionEvent(PipelineProcessorExceptionEventArgs e);
    }
}