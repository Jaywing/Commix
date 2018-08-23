using System;
using System.Collections.Generic;
using System.Text;

using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class EnumProcessor<T> : IPropertyProcesser where T : struct
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
