using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Commix.Pipeline;
using Commix.Pipeline.Mapping;
using Commix.Pipeline.Mapping.Processors;
using Commix.Pipeline.Property;
using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest.Tools
{
    public static class CommixRegistrar
    {
        /// <summary>
        /// Register Commix dependencies
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddCommix(this IServiceCollection serviceCollection, Action<CommixConfiguration> config = null)
        {
            var configurator = new CommixConfiguration(serviceCollection);

            configurator.RegisterProcessors("Commix");

            config?.Invoke(configurator);

            // Default Processor factory
            if (serviceCollection.All(x => x.ServiceType != typeof(IProcessorFactory)))
                configurator.SetProcessorFactory<DefaultProcessorFactory>();

            // Default Pipeline factories
            if (serviceCollection.All(x => x.ServiceType != typeof(IMappingPipelineFactory)))
                configurator.SetMappingPipelineFactory<DefaultMappingPipelineFactory>();

            if (serviceCollection.All(x => x.ServiceType != typeof(IPropertyPipelineFactory)))
                configurator.SetPropertyPipelineFactory<DefaultPropertyPipelineFactory>();

            // Detault Model Pipline processors
            if (serviceCollection.All(x => x.ServiceType != typeof(ISchemeGenerator)))
                serviceCollection.AddTransient<ISchemeGenerator, InMemorySchemaGeneratorProcessor>();

            if (serviceCollection.All(x => x.ServiceType != typeof(IMappingProcessor)))
                serviceCollection.AddTransient<IMappingProcessor, MappingProcessor>();
            
            // Pipelines, intended that these are not replaced, but modified as needed with processors and configured via factory.
            serviceCollection.AddTransient<MappingPipeline>();
            serviceCollection.AddTransient<PropertyPipeline>();

            return serviceCollection;
        }
    }
}
