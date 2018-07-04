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
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<ItemInternalUrlProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}