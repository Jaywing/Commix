﻿using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class ItemNameProcessorExtensions
    {
        /// <summary>
        /// Maps an Item's name
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> ItemName<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Use<ItemNameProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    configure?.Invoke(c);
                }));
        }
    }
}