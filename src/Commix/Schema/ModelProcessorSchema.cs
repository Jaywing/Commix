using System.Collections.Generic;

namespace Commix.Schema
{
    public class ModelProcessorSchema : IPipelineSchema
    {
        public IList<ProcessorSchema> Processors { get; }

        public ModelProcessorSchema(IList<ProcessorSchema> processors) => Processors = processors;
    }
}