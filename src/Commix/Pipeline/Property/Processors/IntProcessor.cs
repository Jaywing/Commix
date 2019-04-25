using System;
using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class IntProcessor : IPropertyProcessor
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case var item when int.TryParse(item.ToString(), out int intValue):
                        pipelineContext.Context = intValue;
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

    public static class IntProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Int<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder) =>
            builder
                .Add(Processor.Property<IntProcessor>());
    }
}