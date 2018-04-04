using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Core.Schema
{
    public class PropertyProcessorSchema
    {
        public Type Type { get; set; }
        public Dictionary<string, object> Options { get; set; }
    }
}