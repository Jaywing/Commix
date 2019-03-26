using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Commix.Pipeline;
using Commix.Pipeline.Mapping;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.Sitecore
{
    public class CommixConfigurator
    {
        private readonly IServiceCollection _serviceCollection;

        public CommixConfigurator(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;

            Options = new CommixOptions();
        }

        internal void SetMappingPipelineFactory<T>() where T : class, IMappingPipelineFactory
            => _serviceCollection.AddSingleton<IMappingPipelineFactory, T>();

        internal void SetPropertyPipelineFactory<T>() where T : class, IPropertyPipelineFactory
            => _serviceCollection.AddSingleton<IPropertyPipelineFactory, T>();

        internal void SetProcessorFactory<T>() where T : class, IProcessorFactory
            => _serviceCollection.AddSingleton<IProcessorFactory, T>();

        public CommixOptions Options { get; }

        /// <summary>
        /// Scans all loaded assemblies with matchin assemble name prefix, registering processors that implement <see cref="IPropertyProcesser"/>
        /// </summary>
        /// <param name="assemblyPrefix">The assembly prefix.</param>
        internal void RegisterProcessors(string assemblyPrefix)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith(assemblyPrefix));

            foreach (Assembly loadedAssembly in assemblies)
                RegisterProcessors(loadedAssembly);
        }

        /// <summary>
        /// Registers the processors.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        internal void RegisterProcessors(Assembly assembly)
        {
            foreach (Type processorType in assembly.GetTypes())
            {
                switch (processorType)
                {
                    case var type when type.IsAbstract || type.IsInterface:
                        continue;
                    case var type when typeof(IPropertyProcesser).IsAssignableFrom(type):
                        _serviceCollection.AddTransient(type);
                        break;
                    case var type when typeof(IModelProcessor).IsAssignableFrom(type):
                        _serviceCollection.AddTransient(type);
                        break;
                }
            }
        }
    }
}