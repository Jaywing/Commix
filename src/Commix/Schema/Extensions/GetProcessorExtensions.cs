using System;

using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class GetProcessorExtensions
    {
        /// <summary>
        /// Set the context from a property on the source with the same property name as the target, i.e. straight through mapping.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Get<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, 
            Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<GetProcessor>(configure));
        }

        /// <summary>
        /// Switch the context to a property on the source with the given alias.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="sourceProperty">The name of property on the source.</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Get<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string sourceProperty, 
            Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<GetProcessor>(c =>
                {
                    c.Option(GetProcessor.SourcePropertyOptionKey, sourceProperty);
                    configure?.Invoke(c);
                }));
        }
    }
}