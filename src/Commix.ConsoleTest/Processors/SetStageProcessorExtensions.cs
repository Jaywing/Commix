using System;
using System.Linq;

using Commix.ConsoleTest.Processors;

namespace Commix.Schema
{
    public static class SetStageProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Set<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<SetProcessor>(configure));
        }
    }
}