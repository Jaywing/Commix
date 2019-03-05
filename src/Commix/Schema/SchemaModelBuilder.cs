namespace Commix.Schema
{
    public class SchemaModelBuilder : SchemeBuilder
    {
        public ModelProcessorSchema Build() =>
            new ModelProcessorSchema
            {
                Processors = Processors
            };
    }
}