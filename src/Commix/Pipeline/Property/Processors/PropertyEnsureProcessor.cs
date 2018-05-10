using System;

using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class PropertyEnsureProcessor : IPropertyProcesser
    {
        public static string EnsureType = $"{typeof(PropertyEnsureProcessor).Name}EnsureType";
        public static string EnsureReplacement = $"{typeof(PropertyEnsureProcessor).Name}EnsureReplacement";
        
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            // Null or Empty value ensure
            switch (pipelineContext.Context)
            {
                case null when processorContext.TryGetOption(EnsureReplacement, out object replacement):
                    pipelineContext.Context = replacement;
                    break;
                case string stringValue when string.IsNullOrEmpty(stringValue) && 
                                             processorContext.TryGetOption(EnsureReplacement, out string replacement):
                    pipelineContext.Context = replacement;
                    break;
                case null:
                    // If there is no replacement safest to just stall the pipline.
                    return;
            }

            // Type ensure
            if (processorContext.TryGetOption(EnsureType, out Type typeToEnsure))
            {
                switch (pipelineContext.Context)
                {
                    case null:
                        return;
                    // Best case we have a replacement of the same type to use
                    case var _ when processorContext.TryGetOption(EnsureReplacement, out object replacement) &&
                                    typeToEnsure.IsInstanceOfType(replacement) &&
                                    !typeToEnsure.IsInstanceOfType(pipelineContext.Context):
                        pipelineContext.Context = replacement;
                        break;
                }
            }

            Next();
        }
    }
}