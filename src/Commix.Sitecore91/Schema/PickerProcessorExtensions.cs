﻿using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class PickerProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Picker<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<PickerProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaContextBuilder<TModel> Picker<TModel>(
            this SchemaContextBuilder<TModel> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<PickerProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}