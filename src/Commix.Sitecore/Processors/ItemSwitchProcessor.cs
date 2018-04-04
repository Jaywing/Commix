using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Core.Pipeline.Property;

using Sitecore.Data;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class ItemSwitchProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<TModel> context)
        {
            if (!(context.ModelMappingContext.Input is Item item))
                throw new InvalidOperationException($"{typeof(FieldSwitchProcessor<TModel>)} expects source of {typeof(Item)}");
            
            switch (context.Value)
            {
                case string stringValue:
                    context.Value = item.Database.GetItem(stringValue);
                    break;
                case ID idValue:
                    context.Value = item.Database.GetItem(idValue);
                    break;
            }

            Next();
        }

        public Dictionary<string, object> Options { get; set; }
    }
}