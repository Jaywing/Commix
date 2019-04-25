using System;
using System.Collections.Generic;
using System.Diagnostics;
using Commix.Diagnostics;

namespace Commix.Pipeline
{
    public class Pipeline<TPipelineContext, TProcessorContext> where TProcessorContext : class
    {
        private readonly List<ProcessorInstance> _processors = new List<ProcessorInstance>();

        public void Add(IProcessor<TPipelineContext, TProcessorContext> pipelineContext, TProcessorContext processorContext)
            => _processors.Add(new ProcessorInstance(pipelineContext, processorContext));

        [DebuggerStepThrough]
        public void Run(TPipelineContext context)
        {
            // If we have a monitor attached, use it to raise telemetry
            var monitor = (context as IMonitoredContext)?.Monitor;

            if (_processors == null || _processors.Count == 0)
                return;

            // Each Processor contains a Next lambda which is an action to execute the next processor in the chain, we simply
            // loop over the processors wiring one to the other sequentially.
            for (int i = 0; i < _processors.Count; i++)
            {
                var stepIndex = i;
                _processors[i].Processor.Next = () =>
                {
                    // Everything inside the lambda is going to execute at pipeline runtime.
                    //
                    // If a processor does not run, in the case of a property processor this can be caused by stage marker state then the processor is
                    // skipped until one runs.
                    //
                    // This does NOT change the original execution order or sequence, meaning that if stage marker state changes on the pipeline skipped processors
                    // can get the chance to run and can run more than once. This is because the stepIndex is captured in the lambda closure. Even it
                    // If 5 steps are skipped

                    while (++stepIndex <= _processors.Count - 1
                           && !RunProcessor(_processors[stepIndex], monitor, context))
                    {
                        // Skipped
                    }
                };
            }

            // Telemetry OnRun
            monitor?.OnRunEvent(new PipelineEventArgs(context));

            try
            {
                RunProcessor(_processors[0], monitor, context);
            }
            catch (Exception exception)
            {
                // Telemetry OnError
                monitor?.OnErrorEvent(new PipelineErrorEventArgs(context, exception));

                throw;
            }

            // Telemetry OnComplete
            monitor?.OnCompleteEvent(new PipelineEventArgs(context));
        }

        protected virtual bool RunProcessor(ProcessorInstance instance, IPipelineMonitor monitor, TPipelineContext context)
        {
            try
            {
                // Telemetry OnProcessorRun
                monitor?.OnProcessorRunEvent(new PipelineProcessorEventArgs(context, instance.Context, instance.Processor.GetType()));

                instance.Processor.Run(context, instance.Context);

                // Telemetry OnProcessorComplete
                monitor?.OnProcessorCompleteEvent(new PipelineProcessorEventArgs(context, instance.Context, instance.Processor.GetType()));
            }
            catch (Exception exception)
            {
                // Telemetry OnProcessorException
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