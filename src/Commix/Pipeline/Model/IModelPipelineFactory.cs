using System.Linq;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// Get the model mapping pipeline.
    /// </summary>
    public interface IModelPipelineFactory
    {
        ModelMappingPipeline GetModelPipeline();

        /// <summary>
        /// Gets the output model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetOutputModel<T>();
    }
}
