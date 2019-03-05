using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class PickerProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Picker<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Property<PickerProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaModelBuilder<TModel> Picker<TModel>(
            this SitecoreHelpers<TModel> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Model<PickerProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}