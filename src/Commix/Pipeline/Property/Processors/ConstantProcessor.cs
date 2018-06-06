using System;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Set the context to a constant value
    /// </summary>
    /// <typeparam name="T">Constant Type</typeparam>
    public class ConstantProcessor<T> : IPropertyProcesser
    {
        public static string ConstantOptionKey = $"{typeof(ConstantProcessor<T>).Name}.ConstantOptionKey";
        
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (processorContext.TryGetOption(ConstantOptionKey, out T constantValue))
                pipelineContext.Context = constantValue;

            Next();
        }
    }
}