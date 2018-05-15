using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

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

        internal void RegisterProcessors(string assemblyPrefix)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith(assemblyPrefix));

            foreach (Assembly loadedAssembly in assemblies)
                RegisterProcessors(loadedAssembly);
        }

        internal void RegisterProcessors(Assembly assembly)
        {
            foreach (Type processorType in assembly.GetTypes())
            {
                switch (processorType)
                {
                    case var type when type.IsAbstract || type.IsInterface:
                        continue;
                    case var type when typeof(IPropertyProcesser).IsAssignableFrom((Type) type):
                        _serviceCollection.AddTransient(type);
                        break;
                }
            }
        }
    }
}