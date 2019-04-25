using Commix.Schema;

namespace Commix.Pipeline.Property
{
    public class PropertyPipeline : Pipeline<PropertyContext, ProcessorSchema>
    {
        protected override bool RunProcessor(ProcessorInstance instance, IPipelineMonitor monitor, PropertyContext context)
        {
            if ((context.Stage & instance.Context.AllowedStages) == context.Stage) return base.RunProcessor(instance, monitor, context);

            return false;
        }
    }
}