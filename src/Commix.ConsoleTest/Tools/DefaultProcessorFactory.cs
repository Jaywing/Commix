using System;
using System.Linq;
using Commix.Pipeline;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest.Tools
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