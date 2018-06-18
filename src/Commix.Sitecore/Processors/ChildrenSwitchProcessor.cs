using System;
using System.Linq;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class ChildrenSwitchProcessor : IPropertyProcesser
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
                        case Item parent:
                            pipelineContext.Context = parent.GetChildren();
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