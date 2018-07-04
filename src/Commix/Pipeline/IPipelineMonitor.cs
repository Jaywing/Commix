using System;
using System.Diagnostics;

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

        [DebuggerStepThrough]
        void OnCompleteEvent(PipelineEventArgs e);
        [DebuggerStepThrough]
        void OnRunEvent(PipelineEventArgs e);
        [DebuggerStepThrough]
        void OnErrorEvent(PipelineErrorEventArgs e);
        [DebuggerStepThrough]
        void OnProcessorRunEvent(PipelineProcessorEventArgs e);
        [DebuggerStepThrough]
        void OnProcessorCompleteEvent(PipelineProcessorEventArgs e);
        [DebuggerStepThrough]
        void OnProcessorExceptionEvent(PipelineProcessorExceptionEventArgs e);
    }
}