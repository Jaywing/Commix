using System;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class EnumProcessor<T> : IPropertyProcessor where T : struct
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case string stringValue when Enum.TryParse(stringValue, true, out T value):
                        pipelineContext.Context = value;
                        break;
                    default:
                        pipelineContext.Faulted = true;
                        break;
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