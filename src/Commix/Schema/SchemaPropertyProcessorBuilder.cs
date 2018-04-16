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

        public void Option(string key, object value)
        {
            _options.Add(key, value);
        }
        
        public PropertyProcessorSchema Build()
        {
            return new PropertyProcessorSchema(Guid.NewGuid(), _processorType, _options);
        }
    }

   
}