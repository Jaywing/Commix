using System;
using System.Collections.Generic;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public class SchemaProcessorBuilder
    {
        private readonly Dictionary<string, object> _options = new Dictionary<string, object>();
        private readonly Type _processorType;
        private PropertyStageMarker _allowedStages = PropertyStageMarker.All;

        public SchemaProcessorBuilder(Type processorType) => _processorType = processorType ?? throw new ArgumentNullException(nameof(processorType));

        public void AddProcessorOption(string key, object value)
        {
            if (_options.ContainsKey(key))
                _options[key] = value;
            else
                _options.Add(key, value);
        }

        public ProcessorSchema Build() =>
            new ProcessorSchema(Guid.NewGuid(), _processorType, _options)
            {
                AllowedStages = _allowedStages
            };

        public void AddAllowedStages(PropertyStageMarker stages)
        {
            _allowedStages = stages;
        }
    }
}