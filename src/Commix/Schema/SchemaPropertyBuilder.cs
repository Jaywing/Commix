using System;
using System.Reflection;

namespace Commix.Schema
{
    public class SchemaPropertyBuilder : SchemeBuilder
    {
        public PropertyInfo PropertyInfo { get; }

        public SchemaPropertyBuilder(PropertyInfo propertyInfo) => PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

        public PropertyPipelineSchema Build() => new PropertyPipelineSchema(Processors, PropertyInfo);
    }
}