using System;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Switches the pipeline context using a provided path, expects the model pipeline source to be a Sitecore item for context.
    /// </summary>
    public class ExplicitItemSwitchProcessor : IPropertyProcesser
    {
        public static string Path = $"{typeof(ExplicitItemSwitchProcessor).Name}Path";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (processorContext.TryGetOption(Path, out string path))
                        pipelineContext.Context = Context.Database.GetItem(path);
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }

            Next();
        }
    }
}