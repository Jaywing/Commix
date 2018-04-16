using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Schema
{
    public class PropertyProcessorSchema
    {
        public Guid InstanceId { get; }
        public Type Type { get; }
        public Dictionary<string, object> Options { get; }

        public PropertyProcessorSchema(Guid instanceId, Type type, Dictionary<string, object> options)
        {
            InstanceId = instanceId;
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}