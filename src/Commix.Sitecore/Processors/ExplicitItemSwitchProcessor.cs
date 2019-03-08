using System;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Switches the pipeline context using a provided path.
    /// </summary>
    public class ExplicitItemSwitchProcessor : IModelProcessor
    {
        public static string IdKey = $"{typeof(ExplicitItemSwitchProcessor).Name}Id";

        public Action Next { get; set; }

        public void Run(ModelContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (processorContext.TryGetOption(IdKey, out ID id))
                        pipelineContext.Context = Context.Database.GetItem(id);
                    else
                        pipelineContext.Faulted = true;
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