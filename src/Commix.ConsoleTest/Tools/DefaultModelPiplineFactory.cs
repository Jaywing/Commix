using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest.Tools
{
    public class DefaultModelPiplineFactory : IModelPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultModelPiplineFactory(IServiceProvider serviceProvider)
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
}