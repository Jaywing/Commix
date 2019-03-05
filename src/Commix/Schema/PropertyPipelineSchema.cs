using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Commix.Schema
{
    public class PropertyPipelineSchema : PipelineSchema
    {
        public PropertyInfo PropertyInfo { get; set; }
        public IList<ProcessorSchema> Processors { get; set; }
    }
}