using System;
using System.Linq;

using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ChildrenSwitchProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> ChildrenSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ChildrenSwitchProcessor>());
        }
    }
}