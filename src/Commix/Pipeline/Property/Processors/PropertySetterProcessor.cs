using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Schema;
using Commix.Tools;

namespace Commix.Pipeline.Property.Processors
{
    public class PropertySetterProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        
        public Func<Task> NextAsync { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (!pipelineContext.Faulted)
                FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.ModelContext.Output, pipelineContext.Context);
        }
    }
}