using Commix.Pipeline.Model;

namespace Commix.Schema
{
    public static class SchemaModelBuilderExtensions
    {
        public static SchemaModelBuilder<TModel> Add<TModel, TProcessor>(
            this SchemaModelBuilder<TModel> builder, ModelProcessorDefinition<TProcessor> processor) where TProcessor : IModelProcessor
        {
            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }
    }
}