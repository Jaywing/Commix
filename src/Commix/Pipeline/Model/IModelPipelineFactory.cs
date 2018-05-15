using System;
using System.Linq;

namespace Commix.Pipeline.Model
{
    public interface IModelPipelineFactory
    {
        ModelMappingPipeline GetModelPipeline();
    }
}
