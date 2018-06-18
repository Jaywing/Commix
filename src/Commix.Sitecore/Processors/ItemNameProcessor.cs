using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore.Processors
{
    public class ItemNameProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    switch (pipelineContext.Context)
                    {
                        case Item item:
                            pipelineContext.Context = item.Name;
                            break;
                        default:
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
