using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public class ProcessorSchema
    {
        public Guid InstanceId { get; }
        public Type Type { get; }
        public Dictionary<string, object> Options { get; }
        public PropertyStageMarker AllowedStages { get; internal set; } = PropertyStageMarker.All;
        
        public ProcessorSchema(Guid instanceId, Type type, Dictionary<string, object> options)
        {
            InstanceId = instanceId;
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}