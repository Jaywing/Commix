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

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (processorContext.TryGetOption(ConstantOptionKey, out T constantValue))
                        pipelineContext.Context = constantValue;
                    else
                        pipelineContext.Faulted = true;
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