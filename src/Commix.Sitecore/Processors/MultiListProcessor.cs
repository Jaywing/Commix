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
            switch (pipelineContext.Value)
            {
                case MultilistField multilistField:
                    pipelineContext.Value = multilistField.GetItems();
                    break;
            }

            Next();
        }
    }
}