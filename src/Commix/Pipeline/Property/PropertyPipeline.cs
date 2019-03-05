using System;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Property
{
    public class PropertyPipeline : Pipeline<PropertyContext, ProcessorSchema>
    {
        protected override bool RunProcessor(ProcessorInstance instance, IPipelineMonitor monitor, PropertyContext context)
        {
            if ((context.Stage & instance.Context.AllowedStages) == context.Stage)
            {
                // Stage allowed, run the processor
                return base.RunProcessor(instance, monitor, context);
            }

            return false;
        }
    }
}