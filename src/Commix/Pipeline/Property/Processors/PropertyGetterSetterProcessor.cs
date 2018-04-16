using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Commix.Schema;
using Commix.Tools;

namespace Commix.Pipeline.Property.Processors
{
    public class PropertyGetterSetterProcessor : IPropertyProcesser
    {
        public static string SourcePropertyOption = $"{typeof(PropertyGetterSetterProcessor).Name}.SourcePropertyOption"; 
        
        public Action Next { get; set; }
        
        public Func<Task> NextAsync { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            PropertyInfo sourcePropertyInfo = GetPropertyInfo(pipelineContext, processorContext);
            if (sourcePropertyInfo != null)
            {
                pipelineContext.Value = FastPropertyAccessor.GetValue(sourcePropertyInfo, pipelineContext.ModelContext.Input);
                Next();
                FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.ModelContext.Output, pipelineContext.Value);
            }
        }

        private PropertyInfo GetPropertyInfo(PropertyContext context, PropertyProcessorSchema processorContext)
        {
            var sourceProperty = context.PropertyInfo.Name;
            if (processorContext.Options.ContainsKey(SourcePropertyOption))
                sourceProperty = (string) processorContext.Options[SourcePropertyOption];
            
            return context.ModelContext.Input.GetType().GetProperty(sourceProperty);;
        }
    }
}