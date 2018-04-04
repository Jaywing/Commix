using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Core.Pipeline.Property.Processors
{
    public class NestedClassProcessor<TModel> : IPropertyMappingProcesser<TModel>
    {
        public static string FactoryTypeOption = $"{typeof(NestedClassProcessor<TModel>).Name}.FactoryTypeOption"; 
        
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<TModel> context)
        {
            if (context.Value != null)
            {
                if (Options.ContainsKey(FactoryTypeOption) && Options[FactoryTypeOption] is Type factoryType)
                {
                    if (Activator.CreateInstance(factoryType) is IAnonymousPipelineRunner pipelineRunner)
                    {
                        context.Value = pipelineRunner.Run(context.Value);

                        Next();
                    }
                }
            }
        }

        public Dictionary<string, object> Options { get; set; }
    }
}
