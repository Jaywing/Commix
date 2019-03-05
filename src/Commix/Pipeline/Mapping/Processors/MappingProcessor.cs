using System;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.Pipeline.Mapping.Processors
{
    /// <summary>
    /// Default model pipeline processor, uses the pipeline schema to map a model.
    /// </summary>
    public class MappingProcessor : IMappingProcessor
    {
        private readonly IProcessorFactory _processorFactory;
        private readonly IPropertyPipelineFactory _propertyPipelineFactory;

        public Action Next { get; set; }

        public MappingProcessor(IProcessorFactory processorFactory, IPropertyPipelineFactory propertyPipelineFactory)
        {
            _processorFactory = processorFactory ?? throw new ArgumentNullException(nameof(processorFactory));
            _propertyPipelineFactory = propertyPipelineFactory ?? throw new ArgumentNullException(nameof(propertyPipelineFactory));
        }

        public void Run(MappingContext pipelineContext, MappingProcessorContext processorContext)
        {
            if (pipelineContext.Schema != null)
            {
                foreach (PipelineSchema schema in pipelineContext.Schema.Schemas)
                {
                    switch (schema)
                    {
                        case PropertyPipelineSchema propertySchema:
                            RunPropertyPipeline(pipelineContext, propertySchema);
                            break;
                        case ModelProcessorSchema mappingSchema:
                            RunModelPipeline(pipelineContext, mappingSchema);
                            break;
                        
                    }
                }
            }

            Next();
        }

        /// <summary>
        /// Builds and runs a property pipeline using a property schema, presets the property pipeline context to the the model pipeline source.
        /// </summary>
        /// <param name="context">Model pipeline context</param>
        /// <param name="propertyPipelineSchema">Property pipeline context</param>
        private void RunPropertyPipeline(MappingContext context, PropertyPipelineSchema propertyPipelineSchema)
        {
            PropertyPipeline propertyPipeline = _propertyPipelineFactory.GetPropertyPipeline();

            foreach (ProcessorSchema propertyProcessorSchema in propertyPipelineSchema.Processors)
            {
                if (_processorFactory.TryGetProcessor(propertyProcessorSchema.Type, out IPropertyProcesser propertyProcesser))
                {
                    // Property processors run on indiviual properties within a Model
                    propertyPipeline.Add(propertyProcesser, propertyProcessorSchema);
                }
                else if (_processorFactory.TryGetProcessor(propertyProcessorSchema.Type, out IModelProcessor modelProcessor))
                {
                    // Model processors run on the model itself
                    propertyPipeline.Add(modelProcessor, propertyProcessorSchema);
                }
            }

            propertyPipeline.Run(new PropertyContext(context, propertyPipelineSchema.PropertyInfo, context.Input) {Monitor = context.Monitor});
        }

        private void RunModelPipeline(MappingContext context, ModelProcessorSchema modelProcessorSchema)
        {
            var modelPipeline = new ModelPipeline();

            foreach (ProcessorSchema schema in modelProcessorSchema.Processors)
            {
                if (_processorFactory.TryGetProcessor(schema.Type, out IModelProcessor contextProcessor))
                    modelPipeline.Add(contextProcessor, schema);
            }

            var modelContext = new ModelContext(context, context.Input);

            modelPipeline.Run(modelContext);

            if (!modelContext.Faulted)
                context.Input = modelContext.Context;
        }
    }
}