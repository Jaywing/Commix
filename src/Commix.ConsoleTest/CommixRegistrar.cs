using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Commix.Diagnostics;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public static class CommixRegistrar
    {
        public static IServiceCollection Commix(this IServiceCollection serviceCollection)
        {
            var commix = new CommixFactories();
            
            serviceCollection.AddSingleton<IModelPipelineFactory>(commix);
            serviceCollection.AddSingleton<IPropertyProcessorFactory>(commix);

            serviceCollection
                .RegisterProcessors("Commix");

            var modelMapperProcessorFactory = new Func<IServiceProvider, IModelMapperProcessor>(provider =>
            {
                var modelMapper = provider.GetRequiredService<ModelMapperProcessor>();
                return new MonitoredModelMapperProcessor(modelMapper);
            });

            serviceCollection.AddTransient<SchemaGeneratorProcessor, InMemorySchemaGeneratorProcessor>();

            serviceCollection.AddSingleton<ModelMapperProcessor>();
            serviceCollection.AddSingleton(modelMapperProcessorFactory);
            
            serviceCollection.AddTransient<ModelMappingPipeline, TestModelMappingPipeline>();

            CommixExtensions.PipelineFactory = commix;

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
}