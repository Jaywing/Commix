using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class StringFieldProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return StringField(builder, fieldId, string.Empty, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId, string defaultValue)
        {
            return StringField(builder, fieldId, defaultValue, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId, string defaultValue, bool disableWebEditing)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<StringFieldProcessor>(c => c
                    .Option(StringFieldProcessor.DisableWebEditingOptionKey, disableWebEditing)))
                .Ensure(typeof(string), defaultValue);
        }
    }
}