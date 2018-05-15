using System;

namespace Commix.Pipeline.Model
{
    public class ModelProcessorContext
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
    }
}