using System;
using System.Collections.Concurrent;
using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    public class InMemorySchemaGeneratorProcessor : SchemaGeneratorProcessor
    {
        private readonly ConcurrentDictionary<Type, ModelSchema> _schemaCache = new ConcurrentDictionary<Type, ModelSchema>();

        protected override ModelSchema BuildSchema(ModelContext context)
            => _schemaCache.GetOrAdd(context.Output.GetType(), type => base.BuildSchema(context));
    }
}