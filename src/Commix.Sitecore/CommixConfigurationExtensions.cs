using Commix.Pipeline.Model;

namespace Commix.Sitecore
{
    public static class CommixConfigurationExtensions
    {
        public static CommixConfiguration ModelPipelineFactory<T>(this CommixConfiguration config)
            where T : class, IModelPipelineFactory
        {
            config.SetModelPipelineFactory<T>();
            return config;
        }

        public static CommixConfiguration PropertyPipelineFactory<T>(this CommixConfiguration config)
            where T : class, IPropertyPipelineFactory
        {
            config.SetPropertyPipelineFactory<T>();
            return config;
        }

        public static CommixConfiguration PropertyProcessorFactory<T>(this CommixConfiguration config)
            where T : class, IPropertyProcessorFactory
        {
            config.SetPropertyProcessorFactory<T>();
            return config;
        }

        public static CommixConfiguration PropertyProcessors(this CommixConfiguration config, string assemblyPrefix)
        {
            config.RegisterProcessors(assemblyPrefix);
            return config;
        }
    }
}