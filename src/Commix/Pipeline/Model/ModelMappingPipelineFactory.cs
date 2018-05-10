using System;
using System.Linq;

using Commix.Pipeline.Property;

namespace Commix.Pipeline.Model
{
    public interface IModelPipelineFactory
    {
        ModelMappingPipeline GetModelPipeline();
    }

    public interface IPropertyPipelineFactory
    {
        PropertyMappingPipeline GetPropertyPipeline();
    }

    public interface IPropertyProcessorFactory
    {
        IPropertyProcesser GetProcessor(Type processorType);
    }
}
