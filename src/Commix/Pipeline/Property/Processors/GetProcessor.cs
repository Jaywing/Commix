using System;
using System.Reflection;

using Commix.Schema;
using Commix.Tools;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Switch context to the value of a property on the model pipeline source.
    /// </summary>
    public class GetProcessor : IPropertyProcesser
    {
        public static string SourcePropertyOptionKey = $"{typeof(GetProcessor).Name}.SourcePropertyOptionKey"; 
        
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted && GetPropertyInfo(pipelineContext, processorContext, out PropertyInfo sourcePropertyInfo))
                {
                    pipelineContext.Context = FastPropertyAccessor.GetValue(sourcePropertyInfo, pipelineContext.ModelContext.Input);
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

        private bool GetPropertyInfo(PropertyContext context, PropertyProcessorSchema processorContext, out PropertyInfo sourcePropertyInfo)
        {
            if (!processorContext.TryGetOption(SourcePropertyOptionKey, out string sourceProperty))
            {
                sourceProperty = context.PropertyInfo.Name;
            }

            sourcePropertyInfo = context.ModelContext.Input.GetType().GetProperty(sourceProperty);;

            return sourcePropertyInfo != null;
        }
    }
}