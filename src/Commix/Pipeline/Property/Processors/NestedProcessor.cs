using System;
using System.Linq;
using Commix.Pipeline.Mapping;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    /// Map a property to a target model type specififed by a processor option
    /// </summary>
    /// <seealso cref="Commix.Pipeline.Property.IPropertyProcesser" />
    public class NestedProcessor : IPropertyProcesser
    {
        private readonly IMappingPipelineFactory _pipelineFactory;

        public NestedProcessor(IMappingPipelineFactory pipelineFactory)
        {
            _pipelineFactory = pipelineFactory ?? throw new ArgumentNullException(nameof(pipelineFactory));
        }

        public static string OutputTypeOption = $"{typeof(NestedProcessor).Name}.OutputType"; 
        
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted && pipelineContext.Context != null)
                {
                    if (processorContext.Options.ContainsKey(OutputTypeOption) && processorContext.Options[OutputTypeOption] is Type outputType)
                    {
                        var mappingContext = new MappingContext(pipelineContext.Context, Activator.CreateInstance(outputType)) {Monitor = pipelineContext.Monitor};

                        MappingPipeline pipeline = _pipelineFactory.GetMappingPipeline();

                        pipeline.Run(mappingContext);
                        pipelineContext.Context = mappingContext.Output;
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
