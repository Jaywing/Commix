using System;
using System.Collections.Concurrent;
using System.Threading;
using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    /// <summary>
    /// Caching schema generator, thread safe.
    /// </summary>
    public class InMemorySchemaGeneratorProcessor : SchemaGeneratorProcessor
    {
        private static readonly ConcurrentDictionary<(int ThreadId, Type TypeId), ModelSchema> SchemaCache = new ConcurrentDictionary<(int ThreadId, Type TypeId), ModelSchema>();

        protected override ModelSchema BuildSchema(ModelContext context)
            => SchemaCache.GetOrAdd((Thread.CurrentThread.ManagedThreadId, context.Output.GetType()), _ => base.BuildSchema(context));
    }
}