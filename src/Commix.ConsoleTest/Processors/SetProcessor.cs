using System;
using System.Linq;

using Commix.ConsoleTest.Tools;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    /// <summary>
    /// Set the model target property value using the current context, unless the property pipeline is flagged as faulted.
    /// </summary>
    public class SetProcessor : IPropertyProcessor
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                    FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.MappingContext.Output, pipelineContext.Context);
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

        public PropertyStageMarker AllowedStages { get; } = PropertyStageMarker.All;
    }
}