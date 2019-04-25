using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public static class SchemaPropertyProcessorBuilderExtensions
    {
        public static SchemaProcessorBuilder Option<T>(this SchemaProcessorBuilder builder, string key, T value)
        {
            builder.AddProcessorOption(key, value);
            return builder;
        }

        public static SchemaProcessorBuilder AllowedStages(this SchemaProcessorBuilder builder, PropertyStageMarker stages)
        {
            builder.AddAllowedStages(stages);
            return builder;
        }
    }
}