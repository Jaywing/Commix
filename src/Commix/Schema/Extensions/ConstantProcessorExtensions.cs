using Commix.Pipeline.Property.Processors;

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class ConstantProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Constant<TModel, TProp, TValue>(
            this SchemaPropertyBuilder<TModel, TProp> builder, TValue value)
        {
            return builder
                .Add(Processor.Use<ConstantProcessor<TValue>>(c => c
                    .Option(ConstantProcessor<TValue>.ConstantOptionKey, value)));
        }
    }
}