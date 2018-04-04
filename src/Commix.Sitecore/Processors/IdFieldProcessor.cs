using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Core.Pipeline.Property;

using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class IdFieldProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<TModel> context)
        {
            if (!(context.Value is Field field))
                throw new InvalidOperationException();

            ID value;
            if (!ID.TryParse(field.GetValue(true), out value))
                throw new InvalidOperationException();
            context.Value = value;
            
            Next();
        }

        public Dictionary<string, object> Options { get; set; }
    }
}