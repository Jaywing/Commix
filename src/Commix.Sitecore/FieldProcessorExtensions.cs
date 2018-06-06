using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Schema.Extensions;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore
{
    public static partial class FieldProcessorExtensions
    {
        /// <summary>
        /// Maps an Item's name
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> ItemName<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ItemNameProcessor>());
        }

        /// <summary>
        /// Map a field's properties onto a model
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Field<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<NestedClassProcessor>(c => c
                    .Option(NestedClassProcessor.OutputTypeOption, typeof(TProp))))
                .Add(Processor.Use<PropertySetProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> ItemUrl<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ItemInternalUrlProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> CheckboxField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<CheckboxFieldProcessor>());
        }

        

        public static SchemaPropertyBuilder<TModel, TProp> FieldSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)));
        }

        public static SchemaPropertyBuilder<TModel, TProp> FieldSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> ChildrenSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ChildrenSwitchProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> MultiList<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<MultiListProcessor>());
        }
    }
}