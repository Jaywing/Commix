﻿using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Core.Diagnostics;
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
                    Monitor?.OnProcessorRunEvent(new PipelineProcessorEventArgs(context, metaProcessor.Context, metaProcessor.Processor.GetType()));

                    metaProcessor.Processor.Run(context, metaProcessor.Context);

                    Monitor?.OnProcessorCompleteEvent(new PipelineProcessorEventArgs(context, metaProcessor.Context, metaProcessor.Processor.GetType()));
                }
                catch (Exception exception)
                {
                    Monitor?.OnProcessorExceptionEvent(new PipelineProcessorExceptionEventArgs(context, exception, metaProcessor.Context, metaProcessor.Processor.GetType()));

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
                Monitor?.OnRunEvent(new PipelineEventArgs(context));

                RunProcessor(_processors[0]);

                Monitor?.OnCompleteEvent(new PipelineEventArgs(context));
            }
            catch (Exception exception)
            {
                Monitor?.OnErrorEvent(new PipelineErrorEventArgs(context, exception));

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