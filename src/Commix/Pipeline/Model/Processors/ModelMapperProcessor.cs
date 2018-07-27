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
        private readonly IProcessorFactory _processorFactory;
        private readonly IPropertyPipelineFactory _propertyPipelineFactory;

        public Action Next { get; set; }

        public ModelMapperProcessor(IProcessorFactory processorFactory, IPropertyPipelineFactory propertyPipelineFactory)
        {
            _processorFactory = processorFactory ?? throw new ArgumentNullException(nameof(processorFactory));
            _propertyPipelineFactory = propertyPipelineFactory ?? throw new ArgumentNullException(nameof(propertyPipelineFactory));
        }

        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
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
                        case ContextProcessorSchema singleProcessorSchema:
                            RunContextPipeline(pipelineContext, singleProcessorSchema);
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
        private void RunPropertyPipeline(ModelContext context, PropertyPipelineSchema propertyPipelineSchema)
        {
            PropertyMappingPipeline propertyPipeline = _propertyPipelineFactory.GetPropertyPipeline();

            foreach (ProcessorSchema propertyProcessorSchema in propertyPipelineSchema.Processors)
            {
                if (_processorFactory.TryGetProcessor(propertyProcessorSchema.Type, out IPropertyProcesser propertyProcesser))
                    propertyPipeline.Add(propertyProcesser, propertyProcessorSchema);
            }

            propertyPipeline.Run(new PropertyContext(context, propertyPipelineSchema.PropertyInfo, context.Input) {Monitor = context.Monitor});
        }

        private void RunContextPipeline(ModelContext context, ContextProcessorSchema contextProcessorSchema)
        {
            var modelPipeline = new NestedMappingPipeline();

            foreach (ProcessorSchema schema in contextProcessorSchema.Processors)
            {
                if (_processorFactory.TryGetProcessor(schema.Type, out INestedProcessor contextProcessor))
                    modelPipeline.Add(contextProcessor, schema);
            }

            var nestedContext = new NestedContext(context, context.Input);

            modelPipeline.Run(nestedContext);

            if (!nestedContext.Faulted)
                context.Input = nestedContext.Context;
        }
    }
}