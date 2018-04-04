using System;
using System.Linq;

using Commix.Core.Pipeline.Model;
using Commix.Core.Pipeline.Property;
using Commix.Core.Pipeline.Property.Processors;
using Commix.Core.Schema;
using Commix.Core.Schema.Extensions;


namespace Commix.Sitecore.Processors
{
    public static class FieldProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId = default(string))
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor<TModel>>(c => c
                    .Option(FieldSwitchProcessor<TModel>.FieldId, fieldId)))
                .Add(Processor.Use<StringFieldProcessor<TModel>>())
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }

        /// <summary>
        /// Maps an Item's name
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> ItemName<TModel, TProp>(this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ItemNameProcessor<TModel>>())
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }

        /// <summary>
        /// Map a field's properties onto a model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Field<TModel, TProp>(this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor<TModel>>(c => c
                    .Option(FieldSwitchProcessor<TModel>.FieldId, fieldId)))
                .Add(Processor.Use<NestedClassProcessor<TModel>>(c => c
                    .Option(NestedClassProcessor<TModel>.FactoryTypeOption, typeof(ModelMappingPipelineFactory<TProp>))))
                .Add(Processor.Use<PropertySetterProcessor<TModel>>());
        }
    }
}