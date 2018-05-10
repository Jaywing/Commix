using System;

using Commix.Pipeline.Property;
using Commix.Schema;
using Sitecore.Data.Fields;

namespace Commix.Sitecore.Processors
{
    public class MultiListProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            switch (pipelineContext.Context)
            {
                case MultilistField multilistField:
                    pipelineContext.Context = multilistField.GetItems();
                    break;
            }

            Next();
        }
    }
}