using System;

namespace Commix.Pipeline.Mapping
{
    public class MappingProcessorContext
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
    }
}