using System;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.Core
{
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
}