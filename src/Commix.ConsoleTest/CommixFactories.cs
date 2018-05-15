using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using Commix.Core.Diagnostics;
using Commix.Diagnostics;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class ConsoleTestModelPiplineFactory : IModelPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly PipelineMonitor _pipelineMonitor;
        private readonly ThreadAwareLogger _tracing;
        
        public ConsoleTestModelPiplineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _pipelineMonitor = new PipelineMonitor();
            _tracing = new ThreadAwareLogger();
        }

        public ModelMappingPipeline GetModelPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<ModelMappingPipeline>();

            var schemaGenerator = _serviceProvider.GetRequiredService<ISchemeGenerator>();
            var modelMapperProcessor = _serviceProvider.GetRequiredService<IModelMapperProcessor>();

            pipeline.Monitor = _pipelineMonitor;

            _tracing.Attach(_pipelineMonitor);

            pipeline.Add(schemaGenerator, new ModelProcessorContext());
            pipeline.Add(modelMapperProcessor, new ModelProcessorContext());

            return pipeline;
        }
    }

    public class ConsoleTestPropertyPipelineFactory : IPropertyPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly PipelineMonitor _pipelineMonitor;
        private readonly ThreadAwareLogger _tracing;

        public ConsoleTestPropertyPipelineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _pipelineMonitor = new PipelineMonitor();
            _tracing = new ThreadAwareLogger();
        }

        public PropertyMappingPipeline GetPropertyPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<PropertyMappingPipeline>();

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