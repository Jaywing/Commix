using System;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Map a property to a target model type specififed by a processor option
    /// </summary>
    /// <seealso cref="Commix.Pipeline.Property.IPropertyProcesser" />
    public class NestedClassProcessor : IPropertyProcesser
    {
        private readonly IModelPipelineFactory _pipelineFactory;

        public NestedClassProcessor(IModelPipelineFactory pipelineFactory)
        {
            _pipelineFactory = pipelineFactory ?? throw new ArgumentNullException(nameof(pipelineFactory));
        }

        public static string OutputTypeOption = $"{typeof(NestedClassProcessor).Name}.OutputType"; 
        
        public Action Next { get; set; }
       
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (pipelineContext.Context != null)
            {
                if (processorContext.Options.ContainsKey(OutputTypeOption) && processorContext.Options[OutputTypeOption] is Type outputType)
                {
                    var mappingContext = new ModelContext(pipelineContext.Context, Activator.CreateInstance(outputType));

                    ModelMappingPipeline pipeline = _pipelineFactory.GetModelPipeline();

                    if (pipeline != null)
                    {
                        pipeline.Run(mappingContext);
                        pipelineContext.Context = mappingContext.Output;

                        Next();
                    }
                }
            }
        }
    }
}
