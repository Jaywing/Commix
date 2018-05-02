using System;
using System.Collections.Concurrent;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class CommixFactories : IModelPipelineFactory, IPropertyProcessorFactory
    {
        private readonly ConcurrentDictionary<(int ThreadId, Type modelType), ModelMappingPipeline> _pipelines = new ConcurrentDictionary<(int ThreadId, Type modelType), ModelMappingPipeline>();

        public ModelMappingPipeline GetPipeline(Type outputType)
        {
            return (ModelMappingPipeline)ServiceLocator.ServiceProvider.GetRequiredService<ModelMappingPipeline>();
            //return _pipelines.GetOrAdd((Thread.CurrentThread.ManagedThreadId, outputType), ServiceLocator.ServiceProvider.GetRequiredService<ModelMappingPipeline>());
        }

        public IPropertyProcesser GetProcessor(Type processorType)
        {
            return (IPropertyProcesser) ServiceLocator.ServiceProvider.GetRequiredService(processorType);
        }
    }
}