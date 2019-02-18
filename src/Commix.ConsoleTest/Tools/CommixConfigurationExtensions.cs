using Commix.Pipeline.Model;

namespace Commix.ConsoleTest.Tools
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

        public static CommixConfiguration ProcessorFactory<T>(this CommixConfiguration config)
            where T : class, IProcessorFactory
        {
            config.SetProcessorFactory<T>();
            return config;
        }

        public static CommixConfiguration Processors(this CommixConfiguration config, string assemblyPrefix)
        {
            config.RegisterProcessors(assemblyPrefix);
            return config;
        }
    }
}