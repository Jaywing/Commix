using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Commix.Core.Pipeline.Property;

using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore.Processors
{
    public class ItemNameProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<TModel> context)
        {
            if (!(context.Value is Item item))
                throw new InvalidOperationException();

            context.Value = item.Name;

            Next();
        }

        public Dictionary<string, object> Options { get; set; }
    }
}
