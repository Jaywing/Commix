using System;

using Commix.Pipeline.Model;

namespace Commix
{
    public static class CommixExtensions
    {
        public static Lazy<IModelPipelineFactory> PipelineFactory  { get; set; }

        /// <summary>
        /// Gets or sets the global pipeline configuration, called just before Model Pipeline execution.
        /// </summary>
        /// <value>
        /// The global pipeline configuration callback.
        /// </value>
        public static Action<ModelMappingPipeline, ModelContext> GlobalPipelineConfig { get; set; }
        
        public static T As<T>(this object source, Action<ModelMappingPipeline, ModelContext> pipelineConfig = null)
        {
            if (PipelineFactory?.Value == null)
                throw new InvalidOperationException("CommixExtensions.PipelineFactory must be set to use static extensions");

            var pipeline = PipelineFactory.Value.GetModelPipeline();
            var output = PipelineFactory.Value.GetOutputModel<T>();
            var context = new ModelContext(source, output);

            GlobalPipelineConfig?.Invoke(pipeline, context);
            pipelineConfig?.Invoke(pipeline, context);

            pipeline.Run(context);

            return output;
        }

        public static object As(this object source, Type modelType, Action<ModelMappingPipeline, ModelContext> pipelineConfig = null)
        {
            if (PipelineFactory?.Value == null)
                throw new InvalidOperationException("CommixExtensions.PipelineFactory must be set to use static extensions");

            var pipeline = PipelineFactory.Value.GetModelPipeline();
            var output = PipelineFactory.Value.GetOutputModel(modelType);
            var context = new ModelContext(source, output);

            GlobalPipelineConfig?.Invoke(pipeline, context);
            pipelineConfig?.Invoke(pipeline, context);

            pipeline.Run(context);

            return output;
        }
    }
}
