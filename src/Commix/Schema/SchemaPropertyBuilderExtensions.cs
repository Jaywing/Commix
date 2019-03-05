using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    // ReSharper restore UnusedTypeParameter

    public static class SchemaPropertyBuilderExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Add<TModel, TProp, TProcessor>(
            this SchemaPropertyBuilder<TModel, TProp> builder, PropertyProcessorDefinition<TProcessor> processor) where TProcessor : IPropertyProcesser
        {
            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }

        public static SchemaPropertyBuilder<TModel, TProp> Add<TModel, TProp, TProcessor>(
            this SchemaPropertyBuilder<TModel, TProp> builder, ModelProcessorDefinition<TProcessor> processor) where TProcessor : IModelProcessor
        {
            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }
    }
}