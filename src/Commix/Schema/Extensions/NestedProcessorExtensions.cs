using System;
using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class NestedProcessorExtensions
    {
        /// <summary>
        ///     Map a nested class, unrelated to source model or property.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Nested<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder,
            Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<NestedProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(NestedProcessor.OutputTypeOption, typeof(TProp));
                    configure?.Invoke(c);
                }));
        }
    }
}