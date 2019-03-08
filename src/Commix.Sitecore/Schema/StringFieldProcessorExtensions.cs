using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;
using Sitecore.Data;

namespace Commix.Sitecore.Schema
{
    public static class StringFieldProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, string fieldId)
        {
            return StringField(builder, fieldId, string.Empty, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, ID fieldId)
        {
            return StringField(builder, fieldId.ToString(), string.Empty, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, string fieldId, string defaultValue)
        {
            return StringField(builder, fieldId, defaultValue, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, ID fieldId, string defaultValue)
        {
            return StringField(builder, fieldId.ToString(), defaultValue, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, string fieldId, string defaultValue, bool disableWebEditing)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Model<FieldSwitchProcessor>(c => c
                    .AllowedStages(PropertyStageMarker.Populating)
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Property<StringFieldProcessor>(c => c
                    .AllowedStages(PropertyStageMarker.Populating)
                    .Option(StringFieldProcessor.DisableWebEditingOptionKey, disableWebEditing)))
                .Ensure(defaultValue);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, ID fieldId, string defaultValue, bool disableWebEditing)
        {
            return StringField(builder, fieldId.ToString(), defaultValue, disableWebEditing);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringFieldRaw<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, string fieldId, string defaultValue)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Model<FieldSwitchProcessor>(c => c
                    .AllowedStages(PropertyStageMarker.Populating)
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Property<StringFieldProcessor>(c => c
                    .AllowedStages(PropertyStageMarker.Populating)
                    .Option(StringFieldProcessor.RawFieldValue, true)))
                .Ensure(defaultValue);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringFieldRaw<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, ID fieldId, string defaultValue)
        {
            return StringFieldRaw(builder, fieldId.ToString(), defaultValue);
        }
    }
}