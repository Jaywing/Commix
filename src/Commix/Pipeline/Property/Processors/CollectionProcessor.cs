using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Exceptions;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Map from one Enumerable to another.
    /// </summary>
    /// <typeparam name="TSource">Source Type</typeparam>
    /// <typeparam name="TTarget">Target Type</typeparam>
    public class CollectionProcessor<TSource, TTarget> : IPropertyProcesser
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (pipelineContext.Context is IEnumerable<TSource> sourceEnumerable)
                    {
                        pipelineContext.Context = sourceEnumerable
                            .Select(i => i.As<TTarget>((pipeline, context) => context.Monitor = pipelineContext.Monitor))
                            .ToList();
                    }
                    else
                    {
                        throw InvalidContextException.Create(pipelineContext);
                    }
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }

            Next();
        }
    }
}