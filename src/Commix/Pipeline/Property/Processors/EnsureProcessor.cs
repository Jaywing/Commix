using System;

using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Ensure a context value based on current value or type, prevents casting errors before setting a property with the current context.
    /// </summary>
    public class EnsureProcessor : IPropertyProcesser
    {
        public static string EnsureType = $"{typeof(EnsureProcessor).Name}EnsureType";
        public static string EnsureReplacement = $"{typeof(EnsureProcessor).Name}EnsureReplacement";
        
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            { 
                // Null or Empty value ensure
                switch (pipelineContext.Context)
                {
                    // If the context is null, and we have a valid replacement switch context to the replacement.
                    case null when processorContext.TryGetOption(EnsureReplacement, out object replacement):
                        pipelineContext.Context = replacement;
                        break;
                    // If the context is an empty or null string, and we have a valid replacement switch context to the replacement.
                    case string stringValue when string.IsNullOrEmpty(stringValue) && 
                                                 processorContext.TryGetOption(EnsureReplacement, out string replacement):
                        pipelineContext.Context = replacement;
                        pipelineContext.Faulted = false;
                        break;
                }

                // Type ensure
                if (processorContext.TryGetOption(EnsureType, out Type typeToEnsure))
                {
                    switch (pipelineContext.Context)
                    {
                        // Best case scenario we have a replacement of the same type to use.
                        case var _ when processorContext.TryGetOption(EnsureReplacement, out object replacement) &&
                                        typeToEnsure.IsInstanceOfType(replacement) &&
                                        !typeToEnsure.IsInstanceOfType(pipelineContext.Context):
                            pipelineContext.Context = replacement;
                            pipelineContext.Faulted = false;
                            break;
                    }
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }
            finally
            {
                Next();
            }
        }
    }
}