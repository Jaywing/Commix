using System;
using System.Linq;
using Commix.Pipeline.Property;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest.Tools
{
    public class DefaultPropertyPipelineFactory : IPropertyPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultPropertyPipelineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public PropertyPipeline GetPropertyPipeline()
        {
            var pipeline = _serviceProvider.GetRequiredService<PropertyPipeline>();

            return pipeline;
        }
    }
}