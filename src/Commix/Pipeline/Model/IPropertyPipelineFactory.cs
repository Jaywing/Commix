using Commix.Pipeline.Property;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// Get the property mapping pipeline.
    /// </summary>
    public interface IPropertyPipelineFactory
    {
        PropertyMappingPipeline GetPropertyPipeline();
    }
}