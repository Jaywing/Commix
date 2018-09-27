using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Commix.Diagnostics;
using Commix.Pipeline;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class ConsoleTestModelPiplineFactory : IModelPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ConsoleTestModelPiplineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ModelMappingPipeline GetModelPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<ModelMappingPipeline>();

            var schemaGenerator = _serviceProvider.GetRequiredService<ISchemeGenerator>();
            var modelMapperProcessor = _serviceProvider.GetRequiredService<IModelMapperProcessor>();

            pipeline.Add(schemaGenerator, new ModelProcessorContext());
            pipeline.Add(modelMapperProcessor, new ModelProcessorContext());

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

        public PropertyMappingPipeline GetPropertyPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<PropertyMappingPipeline>();

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