using System;
using System.Linq;

using Commix.Core.Pipeline.Model.Processors;
using Commix.Core.Pipeline.Property.Processors;

namespace Commix.Core.Pipeline.Model
{
    public class ModelMappingPipelineFactory<T> : IAnonymousPipelineRunner
    {
        public ModelMappingPipeline<T> Create()
        {
            var pipeline = new ModelMappingPipeline<T>();
            
            pipeline.Add(new OutputInitialiseProcessor<T>());
            pipeline.Add(new SchemaGeneratorProcessor<T>());
            pipeline.Add(new ModelMapperProcessor<T>());

            return pipeline;
        }

        public object Run(object source)
        {
            var pipeline = Create();
            var context = new ModelMappingContext<T>(source);

             pipeline.Run(context);

            return context.Output;
        }
    }
}
