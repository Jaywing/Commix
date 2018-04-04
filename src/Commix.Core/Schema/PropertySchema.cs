using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Commix.Core.Schema
{
    public class PropertySchema
    {
        public PropertyInfo PropertyInfo { get; set; }
        public IList<PropertyProcessorSchema> Processors { get; set; }
    }
}