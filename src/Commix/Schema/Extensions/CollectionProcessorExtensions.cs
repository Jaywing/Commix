using System;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class CollectionProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Collection<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder,
            Action<CollectionPropertyBuilder<TModel, TProp>> collection)
        {
            collection(new CollectionPropertyBuilder<TModel, TProp>(builder));

            return builder;
        }
    }
}