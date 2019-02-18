using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ExplicitItemSwitchProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> ExplicitItemSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string pathOrId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<ExplicitItemSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(ExplicitItemSwitchProcessor.Path, pathOrId);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaContextBuilder<TModel> ExplicitItemSwitch<TModel>(
            this SchemaContextBuilder<TModel> builder, string pathOrId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<ExplicitItemSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(ExplicitItemSwitchProcessor.Path, pathOrId);
                    configure?.Invoke(c);
                }));
        }
    }
}