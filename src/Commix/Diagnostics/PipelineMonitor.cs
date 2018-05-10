using System;
using System.Linq;

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
        
        public void OnRunEvent(object pipelineContext)
        {
            RunEvent?.Invoke(this, new PipelineEventArgs(pipelineContext));
        }

        public void OnCompleteEvent(object pipelineContext)
        {
            CompleteEvent?.Invoke(this, new PipelineEventArgs(pipelineContext));
        }

        public void OnErrorEvent(object pipelineContext, Exception exception)
        {
            ErrorEvent?.Invoke(this, new PipelineErrorEventArgs(pipelineContext, exception));
        }

        public void OnRunProcessorEvent(object pipelineContext, object processorContext)
        {
            ProcessorRunEvent?.Invoke(this, new PipelineProcessorEventArgs(pipelineContext, processorContext));
        }

        public void OnCompleteProcessorEvent(object pipelineContext, object processorContext)
        {
            ProcessorCompleteEvent?.Invoke(this, new PipelineProcessorEventArgs(pipelineContext, processorContext));
        }

        public void OnProcessorErrorEvent(object pipelineContext, object processorContext, Exception exception)
        {
            ProcessorExceptionEvent?.Invoke(this, new PipelineProcessorExceptionEventArgs(pipelineContext, processorContext, exception));
        }
    }
}