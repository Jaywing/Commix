using System;

namespace Commix.Pipeline
{
    public interface IPipelineMonitor
    {
        void OnCompleteEvent(object pipelineContext);
        void OnRunEvent(object pipelineContext);
        void OnErrorEvent(object pipelineContext, Exception exception);
        void OnRunProcessorEvent(object pipelineContext, object processorContext);
        void OnCompleteProcessorEvent(object pipelineContext, object processorContext);
        void OnProcessorErrorEvent(object pipelineContext, object processorContext, Exception exception);
    }
}