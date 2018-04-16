using System;
using System.Linq;

namespace Commix.Pipeline.Model
{
    public class ModelMappingPipeline : Pipeline<ModelContext, ModelProcessorContext>
    {

    }

    public class ModelProcessorContext
    {
        // todo: Model Processor options
    }
}