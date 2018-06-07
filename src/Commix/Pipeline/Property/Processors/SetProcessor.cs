using System;
using System.Threading.Tasks;

using Commix.Schema;
using Commix.Tools;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Set the model target property value using the current context, unless the property pipeline is flagged as faulted.
    /// </summary>
    public class SetProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        
        public Func<Task> NextAsync { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (!pipelineContext.Faulted)
            {
                FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.ModelContext.Output,
                    pipelineContext.Context);
            }

            Next();
        }
    }
}