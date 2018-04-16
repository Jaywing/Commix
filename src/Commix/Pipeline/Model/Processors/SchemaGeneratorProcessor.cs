using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    public class SchemaGeneratorProcessor : IAsyncProcessor<ModelContext>,  IProcessor<ModelContext, ModelProcessorContext>
    {
        public Action Next { get; set; }
        public Func<Task> NextAsync { get; set; }

        public async Task Run(ModelContext context, CancellationToken cancellationToken)
        {
            context.Schema = BuildSchema(context);

            await NextAsync();
        }

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

    public class InMemorySchemaGeneratorProcessor : SchemaGeneratorProcessor
    {
        private readonly ConcurrentDictionary<Type, ModelSchema> _schemaCache = new ConcurrentDictionary<Type, ModelSchema>();

        protected override ModelSchema BuildSchema(ModelContext context)
            => _schemaCache.GetOrAdd(context.Output.GetType(), type => base.BuildSchema(context));
    }
}
