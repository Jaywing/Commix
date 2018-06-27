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
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<PickerProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}