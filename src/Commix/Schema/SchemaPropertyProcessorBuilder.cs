﻿using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public class SchemaPropertyProcessorBuilder
    {
        private readonly Type _processorType;
        private readonly Dictionary<string, object> _options = new Dictionary<string, object>();
        private PropertyStageMarker _allowedStages = PropertyStageMarker.All;
        private PropertyStageMarker _stageOnCompletion = PropertyStageMarker.Populating;

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
            return  new PropertyProcessorSchema(Guid.NewGuid(), _processorType, _options)
            {
                AllowedStages = _allowedStages,
                StageOnCompletion = _stageOnCompletion
            };
        }

        public void AddAllowedStages(PropertyStageMarker stages)
        {
            _allowedStages = stages;
        }

        public void SetStageOnCompletion(PropertyStageMarker stage)
        {
            _stageOnCompletion = stage;
        }
    }

    public static class SchemaPropertyProcessorBuilderExtensions
    {
        public static SchemaPropertyProcessorBuilder Option<T>(this SchemaPropertyProcessorBuilder builder, string key, T value)
        {
            builder.AddProcessorOption(key, value);
            return builder;
        }

        public static SchemaPropertyProcessorBuilder AllowedStages(this SchemaPropertyProcessorBuilder builder, PropertyStageMarker stages)
        {
            builder.AddAllowedStages(stages);
            return builder;
        }

        public static SchemaPropertyProcessorBuilder StageOnCompletion(this SchemaPropertyProcessorBuilder builder, PropertyStageMarker stage)
        {
            builder.SetStageOnCompletion(stage);
            return builder;
        }
    }
}