using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Commix.Schema
{
    public class PipelineSchema
    {
       
    }
    
    public class PropertyPipelineSchema : PipelineSchema
    {
        public PropertyInfo PropertyInfo { get; set; }
        public IList<ProcessorSchema> Processors { get; set; }
    }

    public class ContextProcessorSchema : PipelineSchema
    {
        public IList<ProcessorSchema> Processors { get; set; }
    }
}