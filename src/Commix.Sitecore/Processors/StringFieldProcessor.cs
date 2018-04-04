using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Core.Pipeline.Property;

using Sitecore.Data.Fields;

namespace Commix.Sitecore.Processors
{
    public class StringFieldProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<TModel> context)
        {
            if (!(context.Value is Field field))
                throw new InvalidOperationException();

            context.Value = field.GetValue(true);

            Next();
        }

        public Dictionary<string, object> Options { get; set; }
    }


}