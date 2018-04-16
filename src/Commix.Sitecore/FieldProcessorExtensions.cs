using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore
{
    public static class FieldProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId = default(string))
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<StringFieldProcessor>())
                .Add(Processor.Use<PropertySetterProcessor>());
        }

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
                .Add(Processor.Use<ItemNameProcessor>())
                .Add(Processor.Use<PropertySetterProcessor>());
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
                .Add(Processor.Use<PropertySetterProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> ItemUrl<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ItemInternalUrlProcessor>())
                .Add(Processor.Use<PropertySetterProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> CheckboxField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<CheckboxFieldProcessor>())
                .Add(Processor.Use<PropertySetterProcessor>());
        }

        public static SchemaPropertyBuilder<TModel, TProp> ExplicitItemSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string pathOrId)
        {
            return builder
                .Add(Processor.Use<ExplicitItemSwitchProcessor>(c => c
                    .Option(ExplicitItemSwitchProcessor.Path, pathOrId)));
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
    }
}