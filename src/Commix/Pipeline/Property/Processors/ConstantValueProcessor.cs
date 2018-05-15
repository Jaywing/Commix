using System;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class ConstantValueProcessor<T> : IPropertyProcesser
    {
        public static string ConstantValueOption = $"{typeof(ConstantValueProcessor<T>).Name}.ConstantValue";
        
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (processorContext.TryGetOption(ConstantValueOption, out T constantValue))
                pipelineContext.Context = constantValue;

            Next();
        }
    }
}