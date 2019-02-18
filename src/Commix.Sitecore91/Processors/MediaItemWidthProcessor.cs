using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;

namespace Commix.Sitecore.Processors
{
    public class MediaItemWidthProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case ImageField imageField when imageField.MediaItem != null:
                        pipelineContext.Context = imageField.Width;
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