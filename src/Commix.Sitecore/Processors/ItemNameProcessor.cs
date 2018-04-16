using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore.Processors
{
    public class ItemNameProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext context, PropertyProcessorSchema meta)
        {
            if (!(context.Value is Item item))
                throw new InvalidOperationException();

            context.Value = item.Name;

            Next();
        }
    }
}
