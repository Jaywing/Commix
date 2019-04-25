using System;

namespace Commix.Diagnostics
{
    public class PipelineErrorEventArgs : PipelineEventArgs
    {
        public Exception Error { get; }

        public PipelineErrorEventArgs(object context, Exception error)
            : base(context) => Error = error ?? throw new ArgumentNullException(nameof(error));
    }
}