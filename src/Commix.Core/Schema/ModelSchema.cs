using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Core.Schema
{
    public class ModelSchema
    {
        public List<PropertySchema> Properties { get; } = new List<PropertySchema>();
    }
}