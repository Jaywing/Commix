using System;

using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class EnsureProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Ensure<TModel, TProp, TReplacement>(
            this SchemaPropertyBuilder<TModel, TProp> builder, TReplacement replacement, Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<EnsureProcessor>(c =>
                {
                    c.Option(EnsureProcessor.EnsureType, typeof(TReplacement));
                    c.Option(EnsureProcessor.EnsureReplacement, replacement);
                    configure?.Invoke(c);
                }));
        }
    }
}