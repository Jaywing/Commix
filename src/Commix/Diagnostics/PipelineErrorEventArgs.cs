using System;
using System.Linq;

namespace Commix.Core.Diagnostics
{
    public class PipelineErrorEventArgs : PipelineEventArgs
    {
        public Exception Error { get; }

        public PipelineErrorEventArgs(object context, Exception error) 
            : base(context) => Error = error ?? throw new ArgumentNullException(nameof(error));
    }
}