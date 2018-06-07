using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class FieldSwitchProcessorExtensions
    {
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
                .Add(Processor.Use<NestedProcessor>(c => c
                    .Option(NestedProcessor.OutputTypeOption, typeof(TProp))))
                .Add(Processor.Use<SetProcessor>());
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
    }
}