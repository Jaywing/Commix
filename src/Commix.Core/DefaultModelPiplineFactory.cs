using System;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.Core
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
    }
}