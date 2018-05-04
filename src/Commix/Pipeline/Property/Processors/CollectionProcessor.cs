using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class PropertyCollectionProcessor<TSource, TTarget> : IPropertyProcesser
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (pipelineContext.Value is IEnumerable<TSource> sourceEnumerable)
                pipelineContext.Value = sourceEnumerable.Select(i => i.As<TTarget>()).ToList();
            
            Next();
        }
    }
}