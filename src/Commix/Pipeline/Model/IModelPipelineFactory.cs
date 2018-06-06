using System.Linq;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// Get the model mapping pipeline.
    /// </summary>
    public interface IModelPipelineFactory
    {
        ModelMappingPipeline GetModelPipeline();
    }
}
