using System;

using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class EnsureProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Ensure<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Type type, object replacement)
        {
            return builder
                .Add(Processor.Use<EnsureProcessor>(c => c
                    .Option(EnsureProcessor.EnsureType, type)
                    .Option(EnsureProcessor.EnsureReplacement, replacement)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> Ensure<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, object replacement)
        {
            return builder
                .Add(Processor.Use<EnsureProcessor>(c => c
                    .Option(EnsureProcessor.EnsureType, typeof(TProp))
                    .Option(EnsureProcessor.EnsureReplacement, replacement)));
        }
    }
}