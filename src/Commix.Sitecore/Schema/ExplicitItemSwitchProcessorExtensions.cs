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
            this SitecoreHelpers<TModel, TProp> builder, string pathOrId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Model<ExplicitItemSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(ExplicitItemSwitchProcessor.Path, pathOrId);
                    configure?.Invoke(c);
                }));
        }

        public static SchemaModelBuilder<TModel> ExplicitItemSwitch<TModel>(
            this SitecoreHelpers<TModel> builder, string pathOrId, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Model<ExplicitItemSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(ExplicitItemSwitchProcessor.Path, pathOrId);
                    configure?.Invoke(c);
                }));
        }
    }
}