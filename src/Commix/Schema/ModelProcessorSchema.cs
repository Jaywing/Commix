using System.Collections.Generic;

namespace Commix.Schema
{
    public class ModelProcessorSchema : PipelineSchema
    {
        public IList<ProcessorSchema> Processors { get; set; }
    }
}