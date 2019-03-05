using System;

namespace Commix.Pipeline.Mapping
{
    /// <summary>
    /// Get the model mapping pipeline.
    /// </summary>
    public interface IMappingPipelineFactory
    {
        MappingPipeline GetMappingPipeline();

        /// <summary>
        /// Gets the output model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetOutputModel<T>();

        object GetOutputModel(Type modelType);
    }
}
