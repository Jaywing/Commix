using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;

namespace Commix.Sitecore.Processors
{
    public class EnumFieldProcessor<T> : IPropertyProcesser where T : struct
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case Field field when Enum.TryParse(field.Value, true, out T value):
                        pipelineContext.Context = value;
                        break;
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
