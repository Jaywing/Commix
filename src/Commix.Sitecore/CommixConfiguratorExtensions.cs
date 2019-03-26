using Commix.Pipeline;
using Commix.Pipeline.Mapping;
using Commix.Pipeline.Property;

namespace Commix.Sitecore
{
    public static class CommixConfiguratorExtensions
    {
        public static CommixConfigurator ModelPipelineFactory<T>(this CommixConfigurator config)
            where T : class, IMappingPipelineFactory
        {
            config.SetMappingPipelineFactory<T>();
            return config;
        }

        public static CommixConfigurator PropertyPipelineFactory<T>(this CommixConfigurator config)
            where T : class, IPropertyPipelineFactory
        {
            config.SetPropertyPipelineFactory<T>();
            return config;
        }

        public static CommixConfigurator ProcessorFactory<T>(this CommixConfigurator config)
            where T : class, IProcessorFactory
        {
            config.SetProcessorFactory<T>();
            return config;
        }

        public static CommixConfigurator Processors(this CommixConfigurator config, string assemblyPrefix)
        {
            config.RegisterProcessors(assemblyPrefix);
            return config;
        }
    }
}