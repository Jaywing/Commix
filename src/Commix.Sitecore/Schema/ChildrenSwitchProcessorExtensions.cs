using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ChildrenSwitchProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> ChildrenSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<ChildrenSwitchProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}