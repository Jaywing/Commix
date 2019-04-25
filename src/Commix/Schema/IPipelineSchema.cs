using System.Collections.Generic;

namespace Commix.Schema
{
    public interface IPipelineSchema
    {
        IList<ProcessorSchema> Processors { get; }
    }
}