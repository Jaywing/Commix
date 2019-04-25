using System;
using System.Collections.Generic;
using System.Reflection;

namespace Commix.Schema
{
    public class PropertyPipelineSchema : IPipelineSchema
    {
        public IList<ProcessorSchema> Processors { get; }
        public PropertyInfo PropertyInfo { get; }

        public PropertyPipelineSchema(IList<ProcessorSchema> processors, PropertyInfo propertyInfo)
        {
            Processors = processors ?? throw new ArgumentNullException(nameof(processors));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
    }
}