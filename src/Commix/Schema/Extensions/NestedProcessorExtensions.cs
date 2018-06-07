using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class NestedProcessorExtensions
    {
        /// <summary>
        /// Map a nested class, unrelated to source model or property.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Nested<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<NestedProcessor>(c => c
                    .Option(NestedProcessor.OutputTypeOption, typeof(TProp))));
        }

        /// <summary>
        /// Map a nested class, from a property on the source with the same name
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> NestedFrom<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<GetProcessor>())
                .Add(Processor.Use<NestedProcessor>(c => c
                    .Option(NestedProcessor.OutputTypeOption, typeof(TProp))));
        }

        /// <summary>
        /// Map a nested class from a property on the source with the given alias.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="sourceProperty"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> NestedFrom<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string sourceProperty)
        {
            return builder.Get(sourceProperty)
                .Add(Processor.Use<GetProcessor>(o => o
                    .AddProcessorOption(GetProcessor.SourcePropertyOptionKey, sourceProperty)))
                .Add(Processor.Use<NestedProcessor>(c => c
                    .Option(NestedProcessor.OutputTypeOption, typeof(TProp))));
        }
    }
}