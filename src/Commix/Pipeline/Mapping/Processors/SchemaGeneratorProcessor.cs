using System;
using Commix.Schema;

namespace Commix.Pipeline.Mapping.Processors
{
    public class SchemaGeneratorProcessor : ISchemeGenerator
    {
        public Action Next { get; set; }

        public void Run(MappingContext pipelineContext, MappingProcessorContext processorContext)
        {
            pipelineContext.Schema = BuildSchema(pipelineContext);

            Next();
        }

        protected virtual ModelSchema BuildSchema(MappingContext context)
        {
            switch (context.Output)
            {
                case IFluentSchema fluentBuilder:
                {
                    var builder = fluentBuilder.Map();
                    return builder.Build();
                }
            }

            return null;
        }
    }
}