using System;
using System.Linq;
using Commix.Pipeline;


namespace Commix.Sitecore
{
    public class DefaultProcessorFactory : IProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool TryGetProcessor<T>(Type processorType, out T propertyProcessor) where T : class
        {
            propertyProcessor = _serviceProvider.GetService(processorType) as T;
            return propertyProcessor != null;
        }
    }
}