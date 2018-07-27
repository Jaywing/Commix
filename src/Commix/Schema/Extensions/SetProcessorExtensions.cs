using System;

using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class SetProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> SetStage<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, PropertyStageMarker marker, 
            Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<SetStageProcessor>(c =>
                {
                    c.Option(SetStageProcessor.Marker, marker);
                    configure?.Invoke(c);
                }));
        }
    }
}