using System;
using System.Collections.Concurrent;
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
        public ConsolePipelineTrace(int managedThreadId, PipelineMonitor monitor)
            : base(managedThreadId, monitor)
        { }

        protected override void OnRun(EventPattern<PipelineEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId}: Run");
        }

        protected override void OnComplete(EventPattern<PipelineEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId}: Complete");
        }

        protected override void OnError(EventPattern<PipelineErrorEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId} Error: {args.EventArgs.Error.Message}");
        }

        protected override void OnProcessorRun(EventPattern<PipelineProcessorEventArgs> args)
        {
            switch (args.EventArgs.PipelineContext)
            {
                case ModelContext modelContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Model:{args.EventArgs.ProcessorType}) Run");
                    break;
                case PropertyContext propertyContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Prop:{args.EventArgs.ProcessorType}) Run");
                    break;
            }
        }

        protected override void OnProcessorComplete(EventPattern<PipelineProcessorEventArgs> args)
        {
            switch (args.EventArgs.PipelineContext)
            {
                case ModelContext modelContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Model:{args.EventArgs.ProcessorType}) Complete");
                    break;
                case PropertyContext propertyContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Prop:{args.EventArgs.ProcessorType}) Complete");
                    break;
            }
        }

        protected override void OnProcessorError(EventPattern<PipelineProcessorExceptionEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId} Processor Error: {args.EventArgs.Error.Message}");
        }
    }
}