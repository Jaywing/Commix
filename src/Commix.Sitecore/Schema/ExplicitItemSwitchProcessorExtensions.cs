using System;
using System.Linq;

using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ExplicitItemSwitchProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> ExplicitItemSwitch<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string pathOrId)
        {
            return builder
                .Add(Processor.Use<ExplicitItemSwitchProcessor>(c => c
                    .Option(ExplicitItemSwitchProcessor.Path, pathOrId)));
        }
    }
}