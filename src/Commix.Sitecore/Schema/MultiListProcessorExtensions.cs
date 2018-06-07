using System;
using System.Linq;

using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class MultiListProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> MultiList<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return builder
                .Add(Processor.Use<MultiListProcessor>());
        }
    }
}