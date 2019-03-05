using System.Collections.Generic;

namespace Commix.Schema
{
    public class SchemeBuilder
    {
        protected IList<ProcessorSchema> Processors { get; } = new List<ProcessorSchema>();

        public void AddProcessorInfo(ProcessorSchema schema)
        {
            Processors.Add(schema);
        }
    }
}