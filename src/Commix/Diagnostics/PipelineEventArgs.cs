using System;
using System.Linq;

namespace Commix.Diagnostics
{
    public class PipelineEventArgs
    {
        public DateTime Timestamp { get; } = DateTime.Now;
        public object PipelineContext { get; }

        public PipelineEventArgs(object pipelineContext) => PipelineContext = pipelineContext ?? throw new ArgumentNullException(nameof(pipelineContext));
    }
}