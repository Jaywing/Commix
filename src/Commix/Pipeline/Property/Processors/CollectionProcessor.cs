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
            if (pipelineContext.Context is IEnumerable<TSource> sourceEnumerable)
                pipelineContext.Context = sourceEnumerable.Select(i => i.As<TTarget>()).ToList();
            
            Next();
        }
    }
}