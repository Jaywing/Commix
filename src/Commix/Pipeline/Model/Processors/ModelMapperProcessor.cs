using System;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    /// <summary>
    /// Default model pipeline processor, uses the pipeline schema to map a model.
    /// </summary>
    public class ModelMapperProcessor : IModelMapperProcessor
    {
        private readonly IPropertyProcessorFactory _processorFactory;
        private readonly IPropertyPipelineFactory _propertyPipelineFactory;

        public Action Next { get; set; }

        public ModelMapperProcessor(IPropertyProcessorFactory processorFactory, IPropertyPipelineFactory propertyPipelineFactory)
        {
            _processorFactory = processorFactory ?? throw new ArgumentNullException(nameof(processorFactory));
            _propertyPipelineFactory = propertyPipelineFactory ?? throw new ArgumentNullException(nameof(propertyPipelineFactory));
        }

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

        /// <summary>
        /// Builds and runs a property pipeline using a property schema, presets the property pipeline context to the the model pipeline source.
        /// </summary>
        /// <param name="context">Model pipeline context</param>
        /// <param name="propertySchema">Property pipeline context</param>
        private void RunPropertyPipeline(ModelContext context, PropertySchema propertySchema)
        {
            var propertyPipeline = _propertyPipelineFactory.GetPropertyPipeline();

            foreach (PropertyProcessorSchema propertyProcessorSchema in propertySchema.Processors)
            {
                var processor = _processorFactory.GetProcessor(propertyProcessorSchema.Type);
                if (processor is IPropertyProcesser syncProcessor)
                    propertyPipeline.Add(syncProcessor, propertyProcessorSchema);
            }

            propertyPipeline.Run(new PropertyContext(context, propertySchema.PropertyInfo, context.Input) {Monitor = context.Monitor});
        }
    }
}