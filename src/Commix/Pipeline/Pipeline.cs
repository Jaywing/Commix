using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Diagnostics;
using Commix.Pipeline.Model;

namespace Commix.Pipeline
{

    public class Pipeline<TPipelineContext, TProcessorContext> : IObservablePipeline
        where TProcessorContext : class
    {
        public IPipelineMonitor Monitor { get; set; } 
        
        private readonly List<ProcessorInstance> _processors = new List<ProcessorInstance>();
        
        public void Add(IProcessor<TPipelineContext, TProcessorContext> pipelineContext, TProcessorContext processorContext) 
            => _processors.Add(new ProcessorInstance(pipelineContext, processorContext));

        public void Run(TPipelineContext context)
        {
            void RunProcessor(ProcessorInstance metaProcessor)
            {
                try
                {
                    Monitor?.OnRunProcessorEvent(context, metaProcessor.Context);

                    metaProcessor.Processor.Run(context, metaProcessor.Context);

                    Monitor?.OnCompleteProcessorEvent(context, metaProcessor.Context);
                }
                catch (Exception exception)
                {
                    Monitor?.OnProcessorErrorEvent(context, metaProcessor.Context, exception);

                    throw;
                }
            }

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

                        RunProcessor(metaProcessor);
                    }
                };
            }

            try
            {
                Monitor?.OnRunEvent(context);

                RunProcessor(_processors[0]);

                Monitor?.OnCompleteEvent(context);
            }
            catch (Exception exception)
            {
                Monitor?.OnErrorEvent(context, exception);

                throw;
            }
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