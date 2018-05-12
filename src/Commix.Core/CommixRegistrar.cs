using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;
using Microsoft.Extensions.DependencyInjection;

namespace Commix.Core
{
    public static class CommixRegistrar
    {
        public static IServiceCollection AddCommix(this IServiceCollection serviceCollection, Action<CommixConfiguration> config = null)
        {
            var configurator = new CommixConfiguration(serviceCollection);

            config?.Invoke(configurator);

            if (serviceCollection.All(x => x.ServiceType != typeof(IPropertyProcessorFactory)))
                configurator.SetPropertyProcessorFactory<DefaultPropertyProcessorFactory>();

            if (serviceCollection.All(x => x.ServiceType != typeof(IModelPipelineFactory)))
                configurator.SetModelPipelineFactory<DefaultModelPiplineFactory>();

            if (serviceCollection.All(x => x.ServiceType != typeof(IPropertyPipelineFactory)))
                configurator.SetPropertyPipelineFactory<DefaultPropertyPipelineFactory>();

            serviceCollection
                .RegisterProcessors("Commix");

            serviceCollection.AddTransient<ISchemeGenerator, InMemorySchemaGeneratorProcessor>();
            serviceCollection.AddSingleton<IModelMapperProcessor, ModelMapperProcessor>();

            serviceCollection.AddTransient<ModelMappingPipeline>();
            serviceCollection.AddTransient<PropertyMappingPipeline>();

            return serviceCollection;
        }

        public static IServiceCollection RegisterProcessors(this IServiceCollection serviceCollection, string assemblyPrefix)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith(assemblyPrefix));

            foreach (Assembly loadedAssembly in assemblies)
                RegisterProcessors(serviceCollection, loadedAssembly);

            return serviceCollection;
        }

        public static IServiceCollection RegisterProcessors(this IServiceCollection serviceCollection, Assembly assembly)
        {
            foreach (Type processorType in assembly.GetTypes())
            {
                switch (processorType)
                {
                    case var type when type.IsAbstract || type.IsInterface:
                        continue;
                    case var type when typeof(IPropertyProcesser).IsAssignableFrom(type):
                        serviceCollection.AddTransient(type);
                        break;
                }
            }

            return serviceCollection;
        }
    }

    public class DefaultPropertyProcessorFactory : IPropertyProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultPropertyProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPropertyProcesser GetProcessor(Type processorType)
        {
            return (IPropertyProcesser)_serviceProvider.GetRequiredService(processorType);
        }
    }

    public class DefaultModelPiplineFactory : IModelPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultModelPiplineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ModelMappingPipeline GetModelPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<ModelMappingPipeline>();

            var schemaGenerator = _serviceProvider.GetRequiredService<ISchemeGenerator>();
            var modelMapperProcessor = _serviceProvider.GetRequiredService<IModelMapperProcessor>();

            pipeline.Add(schemaGenerator, new ModelProcessorContext());
            pipeline.Add(modelMapperProcessor, new ModelProcessorContext());

            return pipeline;
        }
    }

    public class DefaultPropertyPipelineFactory : IPropertyPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultPropertyPipelineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public PropertyMappingPipeline GetPropertyPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<PropertyMappingPipeline>();

            return pipeline;
        }
    }
}
