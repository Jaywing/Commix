﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;

namespace Commix.Schema
{
    public class SchemaPropertyBuilder : SchemeBuilder
    {
        protected PropertyInfo PropertyInfo { get; set; }
        
        public SchemaPropertyBuilder(PropertyInfo propertyInfo) => PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        
        public PropertyPipelineSchema Build() =>
            new PropertyPipelineSchema
            {
                PropertyInfo = PropertyInfo,
                Processors = Processors
            };
    }
}