using System;
using Commix.Pipeline.Mapping;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    /// <summary>
    ///     Map a property to a target model type specified by a processor option
    /// </summary>
    /// <seealso cref="IPropertyProcessor" />
    public class NestedProcessor : IPropertyProcessor
    {
        public static string OutputTypeOption = $"{typeof(NestedProcessor).Name}.OutputType";
        private readonly IMappingPipelineFactory _pipelineFactory;

        public Action Next { get; set; }

        public NestedProcessor(IMappingPipelineFactory pipelineFactory) => _pipelineFactory = pipelineFactory ?? throw new ArgumentNullException(nameof(pipelineFactory));

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted && pipelineContext.Context != null)
                    if (processorContext.Options.ContainsKey(OutputTypeOption) && processorContext.Options[OutputTypeOption] is Type outputType)
                    {
                        var mappingContext = new MappingContext(pipelineContext.Context, Activator.CreateInstance(outputType)) {Monitor = pipelineContext.Monitor};

                        MappingPipeline pipeline = _pipelineFactory.GetMappingPipeline();

                        pipeline.Run(mappingContext);
                        pipelineContext.Context = mappingContext.Output;
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