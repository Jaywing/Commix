using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Commix.Sitecore.Processors
{
    public class ItemInternalUrlProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            switch (pipelineContext.Value)
            {
                case Item mediaItem when mediaItem.Paths.IsMediaItem:
                    pipelineContext.Value = MediaManager.GetMediaUrl(mediaItem);
                    break;
                case Item contentItem when contentItem.Paths.IsContentItem:
                    pipelineContext.Value = LinkManager.GetItemUrl(contentItem);
                    break;
            }

            Next();
        }
    }
}