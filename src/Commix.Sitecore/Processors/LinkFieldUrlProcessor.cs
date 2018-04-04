using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Core.Pipeline.Property;

using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Commix.Sitecore.Processors
{
    public class LinkFieldUrlProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public Dictionary<string, object> Options { get; set; }
        
        public Action Next { get; set; }

        public void Run(PropertyMappingContext<TModel> context)
        {
            LinkField linkField = context.Value as Field;

            if (linkField != null)
            {
                switch (linkField.LinkType.ToLower())
                {
                    case "internal" when linkField.TargetItem != null:
                        context.Value = LinkManager.GetItemUrl(linkField.TargetItem);
                        break;
                    case "media" when linkField.TargetItem != null:
                        context.Value = MediaManager.GetMediaUrl(linkField.TargetItem);
                        break;
                    case "anchor" when !string.IsNullOrEmpty(linkField.Anchor):
                        context.Value = $"#{linkField.Anchor}";
                        break;
                    default:
                        context.Value = linkField.Url;
                        break;
                }
            }

            Next();
        }
    }
}