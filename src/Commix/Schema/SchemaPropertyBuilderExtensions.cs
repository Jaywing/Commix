using System;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    // ReSharper restore UnusedTypeParameter

    public static class SchemaPropertyBuilderExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Add<TModel, TProp, TProcessor>(
            this SchemaPropertyBuilder<TModel, TProp> builder, PropertyProcessorDefinition<TProcessor> processor) where TProcessor : IPropertyProcessor
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            if (processor.SchemaBuilder == null)
                throw new InvalidOperationException("Processor missing SchemaBuilder");

            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }

        public static SchemaPropertyBuilder<TModel, TProp> Add<TModel, TProp, TProcessor>(
            this SchemaPropertyBuilder<TModel, TProp> builder, ModelProcessorDefinition<TProcessor> processor) where TProcessor : IModelProcessor
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            if (processor.SchemaBuilder == null)
                throw new InvalidOperationException("Processor missing SchemaBuilder");

            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }
    }
}