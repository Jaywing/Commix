using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class ItemSwitchProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (!(pipelineContext.ModelContext.Input is Item item))
                throw new InvalidOperationException($"{typeof(FieldSwitchProcessor)} expects source of {typeof(Item)}");
            
            switch (pipelineContext.Value)
            {
                case string stringValue:
                    pipelineContext.Value = item.Database.GetItem(stringValue);
                    break;
                case ID idValue:
                    pipelineContext.Value = item.Database.GetItem(idValue);
                    break;
                case ReferenceField referenceField:
                    pipelineContext.Value = item.Database.GetItem(referenceField.TargetID);
                    break;
            }

            Next();
        }
    }
}