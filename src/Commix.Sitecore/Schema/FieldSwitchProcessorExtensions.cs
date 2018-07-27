using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class FieldSwitchProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> FieldSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(FieldSwitchProcessor.FieldId, fieldId);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaPropertyBuilder<TModel, TProp> FieldSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaContextBuilder<TModel> FieldSwitch<TModel>(
            this SchemaContextBuilder<TModel> builder, string fieldId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(FieldSwitchProcessor.FieldId, fieldId);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaContextBuilder<TModel> FieldSwitch<TModel>(
            this SchemaContextBuilder<TModel> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}