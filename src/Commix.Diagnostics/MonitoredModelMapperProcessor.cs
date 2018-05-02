using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;

namespace Commix.Diagnostics
{
    public class MonitoredModelMapperProcessor : IModelMapperProcessor
    {
        private readonly ConcurrentDictionary<int, ThreadModelMapTrace> _threadTraces =
            new ConcurrentDictionary<int, ThreadModelMapTrace>();

        private readonly IModelMapperProcessor _processor;

        public MonitoredModelMapperProcessor(IModelMapperProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public Action Next
        {
            get => _processor.Next;
            set => _processor.Next = value;
        }

        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
        {
            if (_processor is IObservableModelMapperProcessor observableProcessor)
            {
                _threadTraces.GetOrAdd(Thread.CurrentThread.ManagedThreadId,
                    managedThreadId => new ThreadModelMapTrace(managedThreadId, observableProcessor));
            }

            _processor.Run(pipelineContext, processorContext);
        }
    }
}