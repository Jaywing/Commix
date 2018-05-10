using System;
using System.Linq;

namespace Commix.Diagnostics
{
    public class PipelineProcessorExceptionEventArgs : PipelineErrorEventArgs
    {
        public object ProcessorContext { get; }

        public PipelineProcessorExceptionEventArgs(object context, object processorContext, Exception error) 
            : base(context, error) => ProcessorContext = processorContext ?? throw new ArgumentNullException(nameof(processorContext));
    }
}