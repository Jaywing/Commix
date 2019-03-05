using System;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public static class Processor
    {
        public static PropertyProcessorDefinition<T> Property<T>(
            Action<SchemaProcessorBuilder> configure = null) where T : IPropertyProcesser => new PropertyProcessorDefinition<T>(configure);

        public static ModelProcessorDefinition<T> Model<T>(
            Action<SchemaProcessorBuilder> configure = null) where T : IModelProcessor => new ModelProcessorDefinition<T>(configure);
    }
}