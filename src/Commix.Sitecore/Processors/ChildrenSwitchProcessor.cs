using System;
using System.Linq;

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
            switch (pipelineContext.Value)
            {
                case Item parent:
                    pipelineContext.Value = parent.GetChildren();
                    break;
            }

            Next();
        }
    }
}