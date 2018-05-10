using System;
using System.Linq;

namespace Commix.Diagnostics
{
    public class PipelineProcessorEventArgs : PipelineEventArgs
    {
        public object ProcessorContext { get; }

        public PipelineProcessorEventArgs(object pipelineContext, object processorContext) 
            : base(pipelineContext) => ProcessorContext = processorContext ?? throw new ArgumentNullException(nameof(processorContext));
    }
}