using System;
using Commix.Pipeline.Model;

namespace Commix.Schema
{
    public class ModelProcessorDefinition<T> where T : IModelProcessor
    {
        public SchemaProcessorBuilder SchemaBuilder { get; } = new SchemaProcessorBuilder(typeof(T));

        public ModelProcessorDefinition(Action<SchemaProcessorBuilder> configure = null) => configure?.Invoke(SchemaBuilder);
    }
}