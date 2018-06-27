using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Commix.Diagnostics;
using Commix.Pipeline.Model;

namespace Commix.Pipeline
{
    public class Pipeline<TPipelineContext, TProcessorContext>
        where TProcessorContext : class
    {
        private readonly List<ProcessorInstance> _processors = new List<ProcessorInstance>();

        public void Add(IProcessor<TPipelineContext, TProcessorContext> pipelineContext, TProcessorContext processorContext) 
            => _processors.Add(new ProcessorInstance(pipelineContext, processorContext));

        [DebuggerStepThrough]
        public void Run(TPipelineContext context)
        {
            var monitor = (context as IMonitoredContext)?.Monitor;

            if (_processors == null || _processors.Count == 0)
                return;

            for (int i = 0; i < _processors.Count; i++)
            {
                var stepIndex = i;
                _processors[i].Processor.Next = () =>
                {
                    while (stepIndex + 1 < _processors.Count)
                    {
                        var metaProcessor = _processors[stepIndex + 1];

                        if (RunProcessor(metaProcessor, monitor, context))
                            break;

                        stepIndex++;

                    }
                };
            }

            monitor?.OnRunEvent(new PipelineEventArgs(context));

            try
            {
                RunProcessor(_processors[0], monitor, context);
            }
            catch (Exception exception)
            {
                monitor?.OnErrorEvent(new PipelineErrorEventArgs(context, exception));

                throw;
            }

            monitor?.OnCompleteEvent(new PipelineEventArgs(context));
        }

        protected virtual bool RunProcessor(ProcessorInstance instance, IPipelineMonitor monitor, TPipelineContext context)
        {
            try
            {
                monitor?.OnProcessorRunEvent(new PipelineProcessorEventArgs(context, instance.Context, instance.Processor.GetType()));

                instance.Processor.Run(context, instance.Context);

                monitor?.OnProcessorCompleteEvent(new PipelineProcessorEventArgs(context, instance.Context, instance.Processor.GetType()));
            }
            catch (Exception exception)
            {
                monitor?.OnProcessorExceptionEvent(new PipelineProcessorExceptionEventArgs(context, exception, instance.Context, instance.Processor.GetType()));
            }

            return true;
        }

        protected class ProcessorInstance
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