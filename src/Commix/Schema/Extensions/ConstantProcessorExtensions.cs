using System;
using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class ConstantProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Constant<TModel, TProp, TValue>(
            this SchemaPropertyBuilder<TModel, TProp> builder, TValue value, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<ConstantProcessor<TValue>>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(ConstantProcessor<TValue>.ConstantOptionKey, value);
                    configure?.Invoke(c);
                }));
        }
    }
}