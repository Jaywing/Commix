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
            
            switch (pipelineContext.Context)
            {
                case string stringValue:
                    pipelineContext.Context = item.Database.GetItem(stringValue);
                    break;
                case ID idValue:
                    pipelineContext.Context = item.Database.GetItem(idValue);
                    break;
                case ReferenceField referenceField:
                    pipelineContext.Context = item.Database.GetItem(referenceField.TargetID);
                    break;
            }

            Next();
        }
    }
}