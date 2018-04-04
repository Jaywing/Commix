using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Core.Pipeline;
using Commix.Core.Pipeline.Property;

using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class FieldSwitchProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public static string FieldId = $"{typeof(FieldSwitchProcessor<TModel>).Name}FieldId";
        
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<TModel> context)
        {
            if (!(context.ModelMappingContext.Input is Item item))
                throw new InvalidOperationException($"{typeof(FieldSwitchProcessor<TModel>)} expects source of {typeof(Item)}");

            string fieldId;
            if (Options.ContainsKey(FieldId))
                fieldId = Options[FieldId].ToString();
            else
                fieldId = context.PropertyInfo.Name;

            var field = item.Fields[fieldId];

            context.Value = field;

            Next();
        }

        public Dictionary<string, object> Options { get; set; }
    }

   
}