using System;
using System.Linq;

using Commix.Pipeline.Property;

namespace Commix.Pipeline.Model
{
    public interface IModelPipelineFactory
    {
        ModelMappingPipeline GetPipeline(Type outputType);
    }

    public interface IPropertyProcessorFactory
    {
        IPropertyProcesser GetProcessor(Type processorType);
    }
}
