using System;

using Commix.Pipeline.Property;
using Commix.Schema;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore.Processors
{
    public class ExplicitItemSwitchProcessor : IPropertyProcesser
    {
        public static string Path = $"{typeof(ExplicitItemSwitchProcessor).Name}Path";
        
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (!(pipelineContext.ModelContext.Input is Item item))
                throw new InvalidOperationException($"{typeof(FieldSwitchProcessor)} expects source of {typeof(Item)}");
            
            if (processorContext.Options.ContainsKey(Path))
                pipelineContext.Context = item.Database.GetItem(processorContext.Options[Path].ToString());

            Next();
        }
    }
}