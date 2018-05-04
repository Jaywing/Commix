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
            Next();
            
            switch (pipelineContext.Value)
            {
                // Pipeline value is null and the value is a value type
                case null when pipelineContext.PropertyInfo.PropertyType.IsValueType:
                    pipelineContext.Value = Activator.CreateInstance(pipelineContext.PropertyInfo.PropertyType);
                    break;
                // pipeline value is not null and the target property type does not match the pipeline value
                case var _ when pipelineContext.Value != null &&
                                pipelineContext.PropertyInfo.PropertyType.IsValueType && 
                                pipelineContext.Value.GetType() != pipelineContext.PropertyInfo.PropertyType:
                    pipelineContext.Value = Activator.CreateInstance(pipelineContext.PropertyInfo.PropertyType);
                    break;
                case var _ when pipelineContext.Value != null &&
                                !pipelineContext.PropertyInfo.PropertyType.IsValueType &&
                                !pipelineContext.PropertyInfo.PropertyType.IsInstanceOfType(pipelineContext.Value):
                    pipelineContext.Value = null;
                    break;
                    
            }
            
            FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.ModelContext.Output, pipelineContext.Value);
        }
    }
}