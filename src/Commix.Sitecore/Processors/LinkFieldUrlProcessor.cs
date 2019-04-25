using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Commix.Sitecore.Processors
{
    public class LinkFieldUrlProcessor : IPropertyProcessor
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (pipelineContext.Context is LinkField linkField)
                    {
                        switch (linkField.LinkType.ToLower())
                        {
                            case "internal" when linkField.TargetItem != null:
                                pipelineContext.Context = LinkManager.GetItemUrl(linkField.TargetItem);
                                break;
                            case "media" when linkField.TargetItem != null:
                                pipelineContext.Context = MediaManager.GetMediaUrl(linkField.TargetItem);
                                break;
                            case "anchor" when !string.IsNullOrEmpty(linkField.Anchor):
                                pipelineContext.Context = $"#{linkField.Anchor}";
                                break;
                            default:
                                pipelineContext.Context = linkField.Url;
                                break;
                        }
                    }
                    else
                    {
                        pipelineContext.Faulted = true;
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