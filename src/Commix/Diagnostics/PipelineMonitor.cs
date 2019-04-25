using System;
using Commix.Pipeline;

namespace Commix.Diagnostics
{
    public class PipelineMonitor : IPipelineMonitor
    {
        public event EventHandler<PipelineEventArgs> RunEvent;
        public event EventHandler<PipelineEventArgs> CompleteEvent;
        public event EventHandler<PipelineErrorEventArgs> ErrorEvent;
        public event EventHandler<PipelineProcessorEventArgs> ProcessorRunEvent;
        public event EventHandler<PipelineProcessorEventArgs> ProcessorCompleteEvent;
        public event EventHandler<PipelineProcessorExceptionEventArgs> ProcessorExceptionEvent;

        public virtual void OnRunEvent(PipelineEventArgs e)
        {
            RunEvent?.Invoke(this, e);
        }

        public virtual void OnCompleteEvent(PipelineEventArgs e)
        {
            CompleteEvent?.Invoke(this, e);
        }

        public virtual void OnErrorEvent(PipelineErrorEventArgs e)
        {
            ErrorEvent?.Invoke(this, e);
        }

        public virtual void OnProcessorRunEvent(PipelineProcessorEventArgs e)
        {
            ProcessorRunEvent?.Invoke(this, e);
        }

        public virtual void OnProcessorCompleteEvent(PipelineProcessorEventArgs e)
        {
            ProcessorCompleteEvent?.Invoke(this, e);
        }

        public virtual void OnProcessorExceptionEvent(PipelineProcessorExceptionEventArgs e)
        {
            ProcessorExceptionEvent?.Invoke(this, e);
        }
    }
}