using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Pipeline
{
    public class Pipeline<TPipelineContext, TProcessorContext>
        where TProcessorContext : class
    {
        private readonly List<ProcessorInstance> _processors = new List<ProcessorInstance>();
        
        public void Add(IProcessor<TPipelineContext, TProcessorContext> pipelineContext, TProcessorContext processorContext) 
            => _processors.Add(new ProcessorInstance(pipelineContext, processorContext));

        public void Run(TPipelineContext context)
        {
            if (_processors == null || _processors.Count == 0)
                return;

            for (int i = 0; i < _processors.Count; i++)
            {
                var stepIndex = i;
                _processors[i].Processor.Next = () =>
                {
                    if (stepIndex + 1 < _processors.Count)
                    {
                        var metaProcessor = _processors[stepIndex + 1];
                        metaProcessor.Processor.Run(context, metaProcessor.Context);
                    }
                };
            }

            _processors[0].Processor.Run(context, _processors[0].Context);
        }

        private class ProcessorInstance
        {
            public IProcessor<TPipelineContext, TProcessorContext> Processor { get; }
            public TProcessorContext Context { get; }

            public ProcessorInstance(IProcessor<TPipelineContext, TProcessorContext> processor, TProcessorContext context)
            {
                Processor = processor ?? throw new ArgumentNullException(nameof(processor));
                Context = context;
            }
        }
    }
}