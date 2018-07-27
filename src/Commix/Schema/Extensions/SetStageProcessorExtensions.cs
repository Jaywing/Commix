using System;

using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class SetStageProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Set<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<SetProcessor>(configure));
        }
    }
}