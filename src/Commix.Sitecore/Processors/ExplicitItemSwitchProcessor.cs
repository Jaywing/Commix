using System;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Switches the pipeline context using a provided path.
    /// </summary>
    public class ExplicitItemSwitchProcessor : IBasicProcessor
    {
        public static string Path = $"{typeof(ExplicitItemSwitchProcessor).Name}Path";

        public Action Next { get; set; }

        public void Run(BasicContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (processorContext.TryGetOption(Path, out string path))
                        pipelineContext.Context = Context.Database.GetItem(path);
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