using System;
using System.Linq;
using System.Reflection;

using Commix.ConsoleTest.Tools;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    /// <summary>
    /// Switch context to the value of a property on the model pipeline source.
    /// </summary>
    public class GetProcessor : IPropertyProcessor
    {
        public static string SourcePropertyOptionKey = $"{typeof(GetProcessor).Name}.SourcePropertyOptionKey";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted && GetPropertyInfo(pipelineContext, processorContext, out PropertyInfo sourcePropertyInfo))
                {
                    pipelineContext.Context = FastPropertyAccessor.GetValue(sourcePropertyInfo, pipelineContext.MappingContext.Input);
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

        private bool GetPropertyInfo(PropertyContext context, ProcessorSchema processorContext, out PropertyInfo sourcePropertyInfo)
        {
            if (!processorContext.TryGetOption(SourcePropertyOptionKey, out string sourceProperty))
            {
                sourceProperty = context.PropertyInfo.Name;
            }

            sourcePropertyInfo = context.MappingContext.Input.GetType().GetProperty(sourceProperty);

            return sourcePropertyInfo != null;
        }
    }
}