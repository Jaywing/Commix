using System;
using System.Linq;
using System.Linq.Expressions;

using Commix.Pipeline.Property.Processors;

namespace Commix.Schema.Extensions
{
    public static partial class ProcessorExtensions
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
                .Add(Processor.Use<NestedClassProcessor>(c => c
                    .Option(NestedClassProcessor.OutputTypeOption, typeof(TProp))))
                .Add(Processor.Use<PropertySetProcessor>());
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
                .Add(Processor.Use<PropertyGetProcessor>())
                .Add(Processor.Use<NestedClassProcessor>(c => c
                    .Option(NestedClassProcessor.OutputTypeOption, typeof(TProp))));
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
                .Add(Processor.Use<PropertyGetProcessor>(o => o
                    .AddProcessorOption(PropertyGetProcessor.SourcePropertyOptionKey, sourceProperty)))
                .Add(Processor.Use<NestedClassProcessor>(c => c
                    .Option(NestedClassProcessor.OutputTypeOption, typeof(TProp))));
        }

        /// <summary>
        /// Set the context from a property on the source with the same property name as the target, i.e. straight through mapping.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Get<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<PropertyGetProcessor>());
        }

        /// <summary>
        /// Switch the context to a property on the source with the given alias.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="sourceProperty">The name of property on the source.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Get<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string sourceProperty)
        {
            return builder
                .Add(Processor.Use<PropertyGetProcessor>(c => c
                    .Option(PropertyGetProcessor.SourcePropertyOptionKey, sourceProperty)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> Set<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<PropertySetProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> Ensure<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Type type, object replacement)
        {
            return builder
                .Add(Processor.Use<PropertyEnsureProcessor>(c => c
                    .Option(PropertyEnsureProcessor.EnsureType, type)
                    .Option(PropertyEnsureProcessor.EnsureReplacement, replacement)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> Ensure<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, object replacement)
        {
            return builder
                .Add(Processor.Use<PropertyEnsureProcessor>(c => c
                    .Option(PropertyEnsureProcessor.EnsureReplacement, replacement)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> Constant<TModel, TProp, TValue>(
            this SchemaPropertyBuilder<TModel, TProp> builder, TValue value)
        {
            return builder
                .Add(Processor.Use<ConstantProcessor<TValue>>(c => c
                    .Option(ConstantProcessor<TValue>.ConstantOptionKey, value)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> Collection<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder,
            Action<CollectionPropertyBuilder<TModel, TProp>> collection)
        {
            collection(new CollectionPropertyBuilder<TModel, TProp>(builder));

            return builder;
        }
    }
}