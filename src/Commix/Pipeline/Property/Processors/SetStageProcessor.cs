using System;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class SetStageProcessor : IPropertyProcessor
    {
        public static string TypeCheck = $"{nameof(SetStageProcessor)}.TypeCheck";
        public static string Marker = $"{nameof(SetStageProcessor)}.Marker";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            if (processorContext.TryGetOption(Marker, out PropertyStageMarker marker))
            {
                if (processorContext.TryGetOption(TypeCheck, out Type checkType))
                {
                    if (checkType.IsInstanceOfType(pipelineContext.Context))
                        pipelineContext.Stage = marker;
                }
                else
                {
                    pipelineContext.Stage = marker;
                }
            }

            Next();
        }
    }
}