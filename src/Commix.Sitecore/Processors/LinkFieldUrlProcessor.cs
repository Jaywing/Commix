using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Commix.Sitecore.Processors
{
    public class LinkFieldUrlProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (pipelineContext.Value is LinkField linkField)
            {
                switch (linkField.LinkType.ToLower())
                {
                    case "internal" when linkField.TargetItem != null:
                        pipelineContext.Value = LinkManager.GetItemUrl(linkField.TargetItem);
                        break;
                    case "media" when linkField.TargetItem != null:
                        pipelineContext.Value = MediaManager.GetMediaUrl(linkField.TargetItem);
                        break;
                    case "anchor" when !string.IsNullOrEmpty(linkField.Anchor):
                        pipelineContext.Value = $"#{linkField.Anchor}";
                        break;
                    default:
                        pipelineContext.Value = linkField.Url;
                        break;
                }
            }

            Next();
        }
    }
}