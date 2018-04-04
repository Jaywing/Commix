using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Core.Schema;

namespace Commix.Core.Pipeline.Model.Processors
{
    public class SchemaGeneratorProcessor<T> : IAsyncProcessor<ModelMappingContext<T>>,  IProcessor<ModelMappingContext<T>>
    {
        public Action Next { get; set; }
        public Func<Task> NextAsync { get; set; }

        public void Run(ModelMappingContext<T> context)
        {
            SetSchema(context);

            Next();
        }

        public async Task Run(ModelMappingContext<T> context, CancellationToken cancellationToken)
        {
            SetSchema(context);

            await NextAsync();
        }

        private void SetSchema(ModelMappingContext<T> context)
        {
            switch (context.Output)
            {
                //case IAsyncFluentSchema<T> fluentBuilder:
                //{
                //    var builder = new AsyncSchemaBuilder<T>();
                //    fluentBuilder.Map(builder);
                //    context.Schema = builder.Build();
                //    break;
                //}
                case IFluentSchema<T> fluentBuilder:
                {
                    var builder = new SchemaBuilder<T>();
                    fluentBuilder.Map(builder);
                    context.Schema = builder.Build();
                    break;
                }
            }
        }
    }
}
