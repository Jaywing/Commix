using System;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Property
{
    public class PropertyMappingPipeline : Pipeline<PropertyContext, PropertyProcessorSchema>
    {
        protected override bool RunProcessor(ProcessorInstance instance, IPipelineMonitor monitor, PropertyContext context)
        {
            if ((context.Stage & instance.Context.AllowedStages) == context.Stage)
            {
                // Stage allowed, run the processor
                var currentNext = instance.Processor.Next;
                
                instance.Processor.Next = () =>
                {
                    context.Stage = instance.Context.StageOnCompletion;
                    currentNext();
                };

                var completed = base.RunProcessor(instance, monitor, context);

                return completed;
            }

            return false;
        }
    }
}