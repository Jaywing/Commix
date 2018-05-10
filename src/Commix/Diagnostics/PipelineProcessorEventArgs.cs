using System;
using System.Linq;

namespace Commix.Diagnostics
{
    public class PipelineProcessorEventArgs : PipelineEventArgs
    {
        public object ProcessorContext { get; }
        public Type ProcessorType { get; }

        public PipelineProcessorEventArgs(object pipelineContext, object processorContext, Type processorType) : base(pipelineContext)
        {
            ProcessorContext = processorContext ?? throw new ArgumentNullException(nameof(processorContext));
            ProcessorType = processorType ?? throw new ArgumentNullException(nameof(processorType));
        }
    }
}