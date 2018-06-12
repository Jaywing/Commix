using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Commix.Diagnostics;
using Commix.Diagnostics.Reactive;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;
using Newtonsoft.Json;

namespace Commix.ConsoleTest
{
    public class NestedPipelineTrace : ThreadedPipelineTrace
    {
        private readonly Stack<PipelineTrace> _traceStack;

        private PipelineTrace _root;

        public NestedPipelineTrace(int managedThreadId)
            : base(managedThreadId)
        {
            _traceStack = new Stack<PipelineTrace>();
        }

        protected override void OnRun(EventPattern<PipelineEventArgs> args)
        {
            var pipelineTrace = new PipelineTrace
            {
                Timestamp = args.EventArgs.Timestamp
            };

            if (_traceStack.Any())
            {
                var currentTrace = _traceStack.Peek();
                currentTrace.ChildTraces.Add(pipelineTrace);
            }
            else
                _root = pipelineTrace;

            _traceStack.Push(pipelineTrace);
        }

        protected override void OnComplete(EventPattern<PipelineEventArgs> args)
        {
            try
            {
                switch (args.EventArgs.PipelineContext)
                {
                    case ModelContext modelContext:
                        PipelineTrace pipelineTrace = _traceStack.Peek();
                        if (pipelineTrace != null)
                        {
                            pipelineTrace.PipelineContext = new
                            {
                                modelContext.Input,
                                modelContext.Output,
                                Schema = new
                                {
                                    modelContext.Schema?.ModelType,
                                    Properties = modelContext.Schema?.Properties?.Select(property => new
                                    {
                                        Processors = property?.Processors?.Select(processor => new
                                        {
                                            processor.Type,
                                            processor.Options
                                        }),
                                        property?.PropertyInfo?.Name,
                                        property?.PropertyInfo?.PropertyType
                                    })
                                }
                            };
                        }

                        break;
                }
            }
            catch (Exception)
            {
                // Best effort
            }

            _traceStack.Pop();

            if (!_traceStack.Any())
            {
                ReverseTraces(_root);
            }
        }

        private void ReverseTraces(PipelineTrace trace)
        {
            foreach (PipelineTrace pipelineTrace in trace.ChildTraces)
            {
                ReverseTraces(pipelineTrace);
            }

            trace.ChildTraces.Reverse();
            trace.Traces.Reverse();
        }

        protected override void OnError(EventPattern<PipelineErrorEventArgs> args)
        {
            _traceStack.Pop();
        }

        protected override void OnProcessorRun(EventPattern<PipelineProcessorEventArgs> args)
        {
            var trace = new ProcessorTrace
            {
                ProcessorType = args.EventArgs.ProcessorType
            };

            switch (args.EventArgs.PipelineContext)
            {
                case PropertyContext processorContext:
                    trace.PreContext = JsonConvert.SerializeObject(processorContext.Context,
                        Formatting.None,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                    break;
            }

            var pipelineStack = _traceStack.Peek();
            var processorStack = pipelineStack.ProcessorStack;

            processorStack.Push(trace);
        }

        protected override void OnProcessorComplete(EventPattern<PipelineProcessorEventArgs> args)
        {
            PipelineTrace pipelineTrace = _traceStack.Peek();
            Stack<ProcessorTrace> processorStack = pipelineTrace.ProcessorStack;
            ProcessorTrace processorTrace = processorStack.Pop();

            switch (args.EventArgs.PipelineContext)
            {
                case PropertyContext processorContext:
                    processorTrace.Faulted = processorContext.Faulted;
                    processorTrace.PostContext = JsonConvert.SerializeObject(processorContext.Context,
                        Formatting.None,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                    break;
            }

            pipelineTrace.Traces.Add(processorTrace);
        }

        protected override void OnProcessorError(EventPattern<PipelineProcessorExceptionEventArgs> args)
        {
            PipelineTrace pipelineTrace = _traceStack.Peek();
            Stack<ProcessorTrace> processorStack = pipelineTrace.ProcessorStack;
            ProcessorTrace processorTrace = processorStack.Pop();

            pipelineTrace.Traces.Add(processorTrace);
        }

        private class PipelineTrace
        {
            [JsonIgnore]
            public Stack<ProcessorTrace> ProcessorStack { get; }

            public DateTime Timestamp { get; set; }

            public object PipelineContext { get; set; }

            public List<PipelineTrace> ChildTraces { get; set; }

            public List<ProcessorTrace> Traces { get; set; }

            public PipelineTrace()
            {
                ProcessorStack = new Stack<ProcessorTrace>();
                Traces = new List<ProcessorTrace>();
                ChildTraces = new List<PipelineTrace>();
            }
        }

        private class ProcessorTrace
        {
            public object PreContext { get; set; }
            public object PostContext { get; set; }
            public Type ProcessorType { get; set; }
            public bool Faulted { get; set; }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_root, Formatting.Indented, 
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}