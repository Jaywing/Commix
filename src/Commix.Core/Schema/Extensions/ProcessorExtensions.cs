using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Commix.Core.Pipeline.Model;
using Commix.Core.Pipeline.Model.Processors;
using Commix.Core.Pipeline.Property;
using Commix.Core.Pipeline.Property.Processors;
using Commix.Core.Tools;

namespace Commix.Core.Schema.Extensions
{
    public static class ProcessorExtensions
    {
        /// <summary>
        /// Map a nested class
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Nested<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<PropertyGetterProcessor<TModel>>())
                .Add(Processor.Use<NestedClassProcessor<TModel>>(c => c
                    .Option(NestedClassProcessor<TModel>.FactoryTypeOption, typeof(ModelMappingPipelineFactory<TProp>))))
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> From<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<PropertyGetterProcessor<TModel>>())
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> From<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Expression<Func<TModel, TProp>> source)
        {
            var propertyInfo = PropertyHelper<TModel>.GetProperty(source);

            return builder
                .Add(Processor.Use<PropertyGetterProcessor<TModel>>(c => c
                    .Option(PropertyGetterProcessor<TModel>.SourcePropertyOption, propertyInfo.Name)))
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> From<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder,
            Expression<Func<TModel, TProp>> source,
            Func<SchemaPropertyBuilder<TModel, TProp>, SchemaPropertyBuilder<TModel, TProp>> wrapped)
        {
            var propertyInfo = PropertyHelper<TModel>.GetProperty(source);

            var prefix = builder
                .Add(Processor.Use<PropertyGetterProcessor<TModel>>(c => c
                    .Option(PropertyGetterProcessor<TModel>.SourcePropertyOption, propertyInfo.Name))
                );

            return wrapped(prefix)
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }
    }
}
