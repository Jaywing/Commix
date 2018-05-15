using System;
using System.Linq;

using Commix.Pipeline.Model;

namespace Commix
{
    public static class CommixExtensions
    {
        public static Lazy<IModelPipelineFactory> PipelineFactory  { get; set; }
        
        public static T As<T>(this object source)
        {
            if (PipelineFactory?.Value == null)
                throw new InvalidOperationException("CommixExtensions.PipelineFactory must be set to use static extensions");

            var pipeline = PipelineFactory.Value.GetModelPipeline();
            var output = Activator.CreateInstance<T>();
            var context = new ModelContext(source, output);
            
            pipeline.Run(context);
            
            return output;
        }
    }
}
