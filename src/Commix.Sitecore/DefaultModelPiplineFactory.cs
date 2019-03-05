using System;
using System.Collections.Generic;
using System.Linq;
using Commix.Pipeline.Mapping;
using Commix.Pipeline.Mapping.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Commix.Sitecore
{
    public class DefaultModelPiplineFactory : IMappingPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultModelPiplineFactory(IServiceProvider serviceProvider)
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
}