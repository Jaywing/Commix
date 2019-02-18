using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class ItemIDProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case IEnumerable<Item> items:
                        pipelineContext.Context = items.Select(x => x.ID).ToList();
                        break;
                    case Item item:
                        pipelineContext.Context = item.ID;
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
