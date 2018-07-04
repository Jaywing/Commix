using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;

using Commix.Diagnostics;
using Commix.Diagnostics.Reactive;
using Commix.Pipeline;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;

namespace Commix.ConsoleTest
{
    public class ConsolePipelineTrace : ThreadedPipelineTrace
    {
        private readonly Stopwatch _pipelineTimer;
        private readonly Stopwatch _processorTimer;

        public ConsolePipelineTrace(int managedThreadId)
            : base(managedThreadId)
        {
            _pipelineTimer = new Stopwatch();
            _processorTimer = new Stopwatch();
        }

        protected override void OnRun(EventPattern<PipelineEventArgs> args)
        {
            _pipelineTimer.Restart();

            Console.WriteLine($"{ManagedThreadId}: Run");
        }

        protected override void OnComplete(EventPattern<PipelineEventArgs> args)
        {
            _pipelineTimer.Stop();

            Console.WriteLine($"{ManagedThreadId}: Complete {_pipelineTimer.Elapsed.TotalMilliseconds}ms");
        }

        protected override void OnError(EventPattern<PipelineErrorEventArgs> args)
        {
            _pipelineTimer.Stop();

            Console.WriteLine($"{ManagedThreadId} Error: {args.EventArgs.Error.Message}");
        }

        protected override void OnProcessorRun(EventPattern<PipelineProcessorEventArgs> args)
        {
            _processorTimer.Restart();

            switch (args.EventArgs.PipelineContext)
            {
                case ModelContext modelContext:
                    Console.WriteLine($"{ManagedThreadId}: Model Processor(Model:{args.EventArgs.ProcessorType}) Run");
                    break;
                case PropertyContext propertyContext:
                    Console.WriteLine($"{ManagedThreadId}: Property Processor(Prop:{args.EventArgs.ProcessorType}) Run");
                    break;
            }
        }

        protected override void OnProcessorComplete(EventPattern<PipelineProcessorEventArgs> args)
        {
            _processorTimer.Stop();

            switch (args.EventArgs.PipelineContext)
            {
                case ModelContext modelContext:
                    Console.WriteLine($"{ManagedThreadId}: Model Processor(Model:{args.EventArgs.ProcessorType}) Complete {_processorTimer.Elapsed.TotalMilliseconds}ms");
                    break;
                case PropertyContext propertyContext:
                    Console.WriteLine($"{ManagedThreadId}: Property Processor(Prop:{args.EventArgs.ProcessorType}) Complete {_processorTimer.Elapsed.TotalMilliseconds}ms");
                    break;
            }
        }

        protected override void OnProcessorError(EventPattern<PipelineProcessorExceptionEventArgs> args)
        {
            _processorTimer.Stop();

            Console.WriteLine($"{ManagedThreadId} Processor Error: {args.EventArgs.Error.Message}");
        }
    }
}