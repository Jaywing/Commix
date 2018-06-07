using System;
using System.Linq;

using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ItemInternalUrlProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> ItemUrl<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<ItemInternalUrlProcessor>());
        }
    }
}