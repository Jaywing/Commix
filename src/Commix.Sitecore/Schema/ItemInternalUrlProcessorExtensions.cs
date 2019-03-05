using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ItemInternalUrlProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> ItemUrl<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .SchemaBuilder
                .Add(Processor.Property<ItemInternalUrlProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}