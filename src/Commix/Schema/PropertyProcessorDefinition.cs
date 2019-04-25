using System;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public class PropertyProcessorDefinition<T> where T : IPropertyProcessor
    {
        public SchemaProcessorBuilder SchemaBuilder { get; } = new SchemaProcessorBuilder(typeof(T));

        public PropertyProcessorDefinition(Action<SchemaProcessorBuilder> configure = null) => configure?.Invoke(SchemaBuilder);
    }
}