using Commix.Pipeline.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Commix.Core
{
    public class CommixConfiguration
    {
        private readonly IServiceCollection _serviceCollection;

        public CommixConfiguration(IServiceCollection serviceCollection) => _serviceCollection = serviceCollection;

        internal void SetModelPipelineFactory<T>() where T : class, IModelPipelineFactory
            => _serviceCollection.AddSingleton<IModelPipelineFactory, T>();

        internal void SetPropertyPipelineFactory<T>() where T : class, IPropertyPipelineFactory
            => _serviceCollection.AddSingleton<IPropertyPipelineFactory, T>();


        internal void SetPropertyProcessorFactory<T>() where T : class, IPropertyProcessorFactory
            => _serviceCollection.AddSingleton<IPropertyProcessorFactory, T>();

    }
}