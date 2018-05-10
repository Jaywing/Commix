using System;
using System.Linq;

namespace Commix.Pipeline
{
    public interface IProcessor<in TPipelineContext, in TProcessorContext>
    {
        Action Next { get; set; }
        void Run(TPipelineContext pipelineContext, TProcessorContext processorContext);
    }
}