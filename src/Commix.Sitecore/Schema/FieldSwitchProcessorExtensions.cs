using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;

using Sitecore.Data;

namespace Commix.Sitecore.Schema
{
    public static class FieldSwitchProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> FieldSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Model<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(FieldSwitchProcessor.FieldId, fieldId);
                    configure?.Invoke(c);
                }));
        }
        public static SchemaPropertyBuilder<TModel, TProp> FieldSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, ID fieldId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Model<FieldSwitchProcessor>(c =>
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
                .Add(Processor.Model<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaModelBuilder<TModel> FieldSwitch<TModel>(
            this SchemaModelBuilder<TModel> builder, string fieldId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Model<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(FieldSwitchProcessor.FieldId, fieldId);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaModelBuilder<TModel> FieldSwitch<TModel>(
            this SchemaModelBuilder<TModel> builder, ID fieldId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Model<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(FieldSwitchProcessor.FieldId, fieldId);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaModelBuilder<TModel> FieldSwitch<TModel>(
            this SchemaModelBuilder<TModel> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Model<FieldSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}