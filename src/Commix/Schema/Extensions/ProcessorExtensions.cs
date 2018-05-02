using System;
using System.Linq;
using System.Linq.Expressions;

using Commix.Pipeline.Property.Processors;

namespace Commix.Schema.Extensions
{
    public static class ProcessorExtensions
    {
        /// <summary>
        /// Map a nested class, unrelated to source model or property, context will be set to the source parent model.
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
                .Add(Processor.Use<PropertySetterProcessor>());
        }
        
        /// <summary>
        /// Map a nested class, from a property on the source with the given alias. with the same name
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> NestedFrom<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Get()
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
                .Add(Processor.Use<PropertyGetterSetterProcessor>());
        }

        /// <summary>
        /// Set the context from a property on the source with the given alias.
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
                .Add(Processor.Use<PropertyGetterSetterProcessor>(c => c
                    .Option(PropertyGetterSetterProcessor.SourcePropertyOption, sourceProperty)));
        }
        

        public static SchemaPropertyBuilder<TModel, TProp> Set<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<PropertySetterProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> GetSet<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<PropertyGetterSetterProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> GetSet<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string sourceProperty)
        {
            return builder
                .Add(Processor.Use<PropertyGetterSetterProcessor>(c => c
                    .Option(PropertyGetterSetterProcessor.SourcePropertyOption, sourceProperty)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> Ensure<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Type type, object replacement)
        {
            return builder
                .Add(Processor.Use<PropertyEnsureProcessor>(c => c
                    .Option(PropertyEnsureProcessor.EnsureType, type)
                    .Option(PropertyEnsureProcessor.EnsureReplacement, replacement)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> ConstantValue<TModel, TProp, TValue>(
            this SchemaPropertyBuilder<TModel, TProp> builder, TValue value)
        {
            return builder
                .Add(Processor.Use<ConstantValueProcessor<TValue>>(c => c
                    .Option(ConstantValueProcessor<TValue>.ConstantValueOption, value)));
        }
    }
}
