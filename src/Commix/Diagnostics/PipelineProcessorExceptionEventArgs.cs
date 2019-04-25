using System;
using System.Diagnostics;

namespace Commix.Diagnostics
{
    [DebuggerStepThrough]
    public class PipelineProcessorExceptionEventArgs : PipelineErrorEventArgs
    {
        public object ProcessorContext { get; }
        public Type ProcessorType { get; }

        public PipelineProcessorExceptionEventArgs(object context, Exception error, object processorContext, Type processorType) : base(context, error)
        {
            ProcessorContext = processorContext ?? throw new ArgumentNullException(nameof(processorContext));
            ProcessorType = processorType ?? throw new ArgumentNullException(nameof(processorType));
        }
    }
}