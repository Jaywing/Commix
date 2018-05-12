using System;

using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    public class SchemaGeneratorProcessor : ISchemeGenerator
    {
        public Action Next { get; set; }

        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
        {
            pipelineContext.Schema = BuildSchema(pipelineContext);

            Next();
        }

        protected virtual ModelSchema BuildSchema(ModelContext context)
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
