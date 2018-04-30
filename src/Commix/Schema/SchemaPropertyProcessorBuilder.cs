using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Schema
{
    public class SchemaPropertyProcessorBuilder
    {
        private readonly Type _processorType;
        private readonly Dictionary<string, object> _options = new Dictionary<string, object>();

        public SchemaPropertyProcessorBuilder(Type processorType)
        {
            _processorType = processorType ?? throw new ArgumentNullException(nameof(processorType));
        }

        public void AddProcessorOption(string key, object value)
        {
            if (_options.ContainsKey(key))
                _options[key] = value;
            else
                _options.Add(key, value);
        }
        
        public PropertyProcessorSchema Build()
        {
            return new PropertyProcessorSchema(Guid.NewGuid(), _processorType, _options);
        }
    }

    public static class SchemaPropertyProcessorBuilderExtensions
    {
        public static SchemaPropertyProcessorBuilder Option<T>(this SchemaPropertyProcessorBuilder builder, string key, T value)
        {
            builder.AddProcessorOption(key, value);
            return builder;
        } 
    }
}