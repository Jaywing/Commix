using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;

namespace Commix.Schema
{
    public class CollectionPropertyBuilder<TModel, TProp>
    {
        private readonly SchemaPropertyBuilder<TModel, TProp> _schemaBuilder;

        public CollectionPropertyBuilder(SchemaPropertyBuilder<TModel, TProp> schemaBuilder)
            => _schemaBuilder = schemaBuilder ?? throw new ArgumentNullException(nameof(schemaBuilder));

        public void Define<TSource, TTarget>()
        {
            _schemaBuilder.Add(Processor.Use<PropertyCollectionProcessor<TSource, TTarget>>());
        }
    }
}
