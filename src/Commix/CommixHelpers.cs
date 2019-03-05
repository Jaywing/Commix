using System;
using Commix.Pipeline.Mapping;

namespace Commix
{
    public static class CommixExtensions
    {
        public static Lazy<IMappingPipelineFactory> PipelineFactory  { get; set; }

        /// <summary>
        /// Gets or sets the global pipeline configuration, called just before Model Pipeline execution.
        /// </summary>
        /// <value>
        /// The global pipeline configuration callback.
        /// </value>
        public static Action<MappingPipeline, MappingContext> GlobalPipelineConfig { get; set; }
        
        public static T As<T>(this object source, Action<MappingPipeline, MappingContext> pipelineConfig = null)
        {
            if (PipelineFactory?.Value == null)
                throw new InvalidOperationException("CommixExtensions.PipelineFactory must be set to use static extensions");

            var pipeline = PipelineFactory.Value.GetMappingPipeline();
            var output = PipelineFactory.Value.GetOutputModel<T>();
            var context = new MappingContext(source, output);

            GlobalPipelineConfig?.Invoke(pipeline, context);
            pipelineConfig?.Invoke(pipeline, context);

            pipeline.Run(context);

            return output;
        }

        public static object As(this object source, Type modelType, Action<MappingPipeline, MappingContext> pipelineConfig = null)
        {
            if (PipelineFactory?.Value == null)
                throw new InvalidOperationException("CommixExtensions.PipelineFactory must be set to use static extensions");

            var pipeline = PipelineFactory.Value.GetMappingPipeline();
            var output = PipelineFactory.Value.GetOutputModel(modelType);
            var context = new MappingContext(source, output);

            GlobalPipelineConfig?.Invoke(pipeline, context);
            pipelineConfig?.Invoke(pipeline, context);

            pipeline.Run(context);

            return output;
        }
    }
}
