using System;
using System.Linq;

using Commix.Exceptions;
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

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    switch (pipelineContext.Context)
                    {
                        case Item mediaItem when mediaItem.Paths.IsMediaItem:
                            pipelineContext.Context = MediaManager.GetMediaUrl(mediaItem);
                            break;
                        case Item contentItem when contentItem.Paths.IsContentItem:
                            pipelineContext.Context = LinkManager.GetItemUrl(contentItem);
                            break;
                        default:
                            pipelineContext.Faulted = true;
                            break;
                    }
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