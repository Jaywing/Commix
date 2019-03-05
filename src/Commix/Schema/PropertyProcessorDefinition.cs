using System;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    //public class ProcesserDefinition<TProcessor>
    //{
    //    public SchemeBuilder SchemaBuilder { get; } = new SchemeBuilder(typeof(TProcessor));

    //    public ProcesserDefinition(Action<SchemaBuilder> configure = null) 
    //        => configure?.Invoke(SchemaBuilder);
    //}

    public class PropertyProcessorDefinition<T> where T: IPropertyProcesser
    {
        public SchemaProcessorBuilder SchemaBuilder { get; } = new SchemaProcessorBuilder(typeof(T));

        public PropertyProcessorDefinition(Action<SchemaProcessorBuilder> configure = null) => configure?.Invoke(SchemaBuilder);
    }
}