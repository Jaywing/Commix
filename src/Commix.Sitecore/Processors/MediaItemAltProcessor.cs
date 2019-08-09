using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class MediaItemAltProcessor : IPropertyProcessor
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case ImageField imageField when imageField.MediaItem != null:
                        pipelineContext.Context = imageField.Alt ?? imageField.MediaItem.DisplayName ?? imageField.MediaItem.Name;
                        break;
                    case MediaItem mediaItem:
                        pipelineContext.Context = mediaItem.DisplayName ?? mediaItem.Name;
                        break;
                    case var item when item is MediaItem mediaItem:
                        pipelineContext.Context = mediaItem.DisplayName ?? mediaItem.Name;
                        break;
                    default:
                        pipelineContext.Faulted = true;
                        break;
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }
            finally
            {
                Next();
            }
        }
    }
}
