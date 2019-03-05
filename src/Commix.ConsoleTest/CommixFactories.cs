using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Commix.Diagnostics;
using Commix.Pipeline;
using Commix.Pipeline.Mapping;
using Commix.Pipeline.Mapping.Processors;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class ConsoleTestModelPiplineFactory : IMappingPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ConsoleTestModelPiplineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public MappingPipeline GetMappingPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<MappingPipeline>();

            var schemaGenerator = _serviceProvider.GetRequiredService<ISchemeGenerator>();
            var modelMapperProcessor = _serviceProvider.GetRequiredService<IMappingProcessor>();

            pipeline.Add(schemaGenerator, new MappingProcessorContext());
            pipeline.Add(modelMapperProcessor, new MappingProcessorContext());

            return pipeline;
        }

        public T GetOutputModel<T>()
        {
            var model = _serviceProvider.GetService<T>();
            if (EqualityComparer<T>.Default.Equals(model, default(T)))
                model = Activator.CreateInstance<T>();
            return model;
        }

        public object GetOutputModel(Type modelType)
        {
            return _serviceProvider.GetService(modelType) 
                   ?? Activator.CreateInstance(modelType);
        }
    }

    public class ConsoleTestPropertyPipelineFactory : IPropertyPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ConsoleTestPropertyPipelineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public PropertyPipeline GetPropertyPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<PropertyPipeline>();

            return pipeline;
        }
    }

    public class ThreadAwareLogger
    {
        private readonly ConcurrentDictionary<int, ConsolePipelineTrace> _threadTraces =
            new ConcurrentDictionary<int, ConsolePipelineTrace>();

        public void Attach(IPipelineMonitor monitor)
        {
            _threadTraces.GetOrAdd(Thread.CurrentThread.ManagedThreadId,
                managedThreadId =>
                {
                    var trace = new ConsolePipelineTrace(managedThreadId);
                    trace.Attach(monitor);
                    return trace;
                });
        }
    }
}