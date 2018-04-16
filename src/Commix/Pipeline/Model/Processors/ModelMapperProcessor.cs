using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    public class ModelMapperProcessor : IAsyncProcessor<ModelContext>, IProcessor<ModelContext, ModelProcessorContext>
    {
        private readonly IPropertyProcessorFactory _processorFactory;

        public ModelMapperProcessor(IPropertyProcessorFactory processorFactory) => _processorFactory = processorFactory ?? throw new ArgumentNullException(nameof(processorFactory));

        public Action Next { get; set; }
        
        #region Sync

        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
        {
            if (pipelineContext.Schema != null)
            {
                foreach (var propertySchema in pipelineContext.Schema.Properties)
                {
                    RunPropertyPipeline(pipelineContext, propertySchema);
                }
            }

            Next();
        }

        private void RunPropertyPipeline(ModelContext context, PropertySchema propertySchema)
        {
            var propertyPipeline = new PropertyMappingPipeline();
            foreach (PropertyProcessorSchema propertyProcessorSchema in propertySchema.Processors)
            {
                var processor = _processorFactory.GetProcessor(propertyProcessorSchema.Type);
                if (processor is IPropertyProcesser syncProcessor)
                {
                    propertyPipeline.Add(syncProcessor, propertyProcessorSchema);
                }
            }

            propertyPipeline.Run(CreatePropertyContext(context, propertySchema));
        }

        private PropertyContext CreatePropertyContext(ModelContext context, PropertySchema propertySchema)
        {
            return new PropertyContext(context, propertySchema.PropertyInfo) { Value = context.Input };
        }

        #endregion

        #region Async

        public Func<Task> NextAsync { get; set; }

        public async Task Run(ModelContext context, CancellationToken cancellationToken)
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

        private async Task RunPropertyPipelineAsync(ModelContext context, PropertySchema propertySchema, CancellationToken cancellationToken)
        {
            var propertyPipeline = new AsyncPropertyMappingPipeline();
            foreach (var propertyProcessorSchemas in propertySchema.Processors)
            {
                var processor = _processorFactory.GetProcessor(propertyProcessorSchemas.Type);
                if (processor is IAsyncPropertyMappingProcesser asyncProcessor)
                {
                    propertyPipeline.Add(asyncProcessor);
                }
            }

            await propertyPipeline.Run(CreatePropertyContext(context, propertySchema), cancellationToken);
        }

        #endregion
    }
}