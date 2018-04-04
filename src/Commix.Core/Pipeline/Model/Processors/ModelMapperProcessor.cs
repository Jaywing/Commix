using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Core.Pipeline.Property;
using Commix.Core.Schema;

namespace Commix.Core.Pipeline.Model.Processors
{
    public class ModelMapperProcessor<TModel> : IAsyncProcessor<ModelMappingContext<TModel>>, IProcessor<ModelMappingContext<TModel>>
    {
        public Action Next { get; set; }

        #region Sync

        public void Run(ModelMappingContext<TModel> context)
        {
            if (context.Schema != null)
            {
                foreach (var propertySchema in context.Schema.Properties)
                {
                    RunPropertyPipeline(context, propertySchema);
                }
            }

            Next();
        }

        private void RunPropertyPipeline(ModelMappingContext<TModel> context, PropertySchema propertySchema)
        {
            var propertyPipeline = new PropertyMappingPipeline<TModel>();

            foreach (var propertyProcessorSchemas in propertySchema.Processors)
            {
                var processor = Activator.CreateInstance(propertyProcessorSchemas.Type);
                if (processor is IPropertyMappingProcesser<TModel> syncProcessor)
                {
                    syncProcessor.Options = propertyProcessorSchemas.Options;
                    propertyPipeline.Add(syncProcessor);
                }
            }

            propertyPipeline.Run(CreatePropertyContext(context, propertySchema));
        }

        private PropertyMappingContext<TModel> CreatePropertyContext(ModelMappingContext<TModel> context, PropertySchema propertySchema)
        {
            var propertyMappingContext = new PropertyMappingContext<TModel>()
            {
                ModelMappingContext = context,
                PropertyInfo = propertySchema.PropertyInfo,
                Value = context.Input
            };
            return propertyMappingContext;
        }

        #endregion

        #region Async

        public Func<Task> NextAsync { get; set; }

        public async Task Run(ModelMappingContext<TModel> context, CancellationToken cancellationToken)
        {
            if (context.Schema != null)
            {
                foreach (var propertySchema in context.Schema.Properties)
                {
                    await RunPropertyPipelineAsync(context, propertySchema, cancellationToken);
                }
            }

            await NextAsync();
        }

        private async Task RunPropertyPipelineAsync(ModelMappingContext<TModel> context, PropertySchema propertySchema, CancellationToken cancellationToken)
        {
            var propertyPipeline = new AsyncPropertyMappingPipeline<TModel>();

            foreach (var propertyProcessorSchemas in propertySchema.Processors)
            {
                var processor = Activator.CreateInstance(propertyProcessorSchemas.Type);
                if (processor is IAsyncPropertyMappingProcesser<TModel> asyncProcessor)
                {
                    asyncProcessor.Options = propertyProcessorSchemas.Options;
                    propertyPipeline.Add(asyncProcessor);
                }
            }

            await propertyPipeline.Run(CreatePropertyContext(context, propertySchema), cancellationToken);
        }

        #endregion
    }
}