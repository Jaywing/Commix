using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using Commix.Diagnostics;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class CommixFactories : IModelPipelineFactory, IPropertyProcessorFactory, IPropertyPipelineFactory
    {
        private readonly PipelineMonitor _pipelineMonitor;
        private readonly ThreadAwareLogger _tracing;

        public CommixFactories()
        {
            _pipelineMonitor = new PipelineMonitor();
            _tracing = new ThreadAwareLogger();
        }

        public ModelMappingPipeline GetModelPipeline()
        {
            var pipeline = ServiceLocator.ServiceProvider.GetRequiredService<ModelMappingPipeline>();
            
            var schemaGenerator = ServiceLocator.ServiceProvider.GetRequiredService<SchemaGeneratorProcessor>();
            var modelMapperProcessor = ServiceLocator.ServiceProvider.GetRequiredService<IModelMapperProcessor>();

            pipeline.Monitor = _pipelineMonitor;
            
            _tracing.Attach(_pipelineMonitor);

            pipeline.Add(schemaGenerator, new ModelProcessorContext());
            pipeline.Add(modelMapperProcessor, new ModelProcessorContext());

            return pipeline;
        }

        public IPropertyProcesser GetProcessor(Type processorType)
        {
            return (IPropertyProcesser) ServiceLocator.ServiceProvider.GetRequiredService(processorType);
        }

        public PropertyMappingPipeline GetPropertyPipeline()
        {
            var pipeline = ServiceLocator.ServiceProvider.GetRequiredService<PropertyMappingPipeline>();
            
            pipeline.Monitor = _pipelineMonitor;

            _tracing.Attach(_pipelineMonitor);
            
            return pipeline;
        }
    }

    public class ThreadAwareLogger
    {
        private readonly ConcurrentDictionary<int, ConsolePipelineTrace> _threadTraces =
            new ConcurrentDictionary<int, ConsolePipelineTrace>();

        public void Attach(PipelineMonitor monitor)
        {
            _threadTraces.GetOrAdd(Thread.CurrentThread.ManagedThreadId,
                managedThreadId => new ConsolePipelineTrace(managedThreadId, monitor));
        }
    }
}