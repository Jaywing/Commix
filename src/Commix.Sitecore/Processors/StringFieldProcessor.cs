﻿using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Sitecore.Data.Fields;

namespace Commix.Sitecore.Processors
{
    public class StringFieldProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (pipelineContext.Value is TextField field)
            {
                pipelineContext.Value = field.Value;
            }

            Next();
        }
    }
}