using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commix.Core.Pipeline.Model.Processors
{
    public class OutputInitialiseProcessor<T> : IProcessor<ModelMappingContext<T>>, IAsyncProcessor<ModelMappingContext<T>>
    {
        private readonly Func<T> _factory;

        public OutputInitialiseProcessor() : this(Activator.CreateInstance<T>) { }
        public OutputInitialiseProcessor(Func<T> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public Action Next { get; set; }
        public Func<Task> NextAsync { get; set; }

        private void InitialiseOutput(ModelMappingContext<T> context)
        {
            switch (typeof(T))
            {
                //case var type when type.IsValueType && !EqualityComparer.Default.Equals(context.Output, default(T)):
                //    context.Output = default(T);
                //    break;
                case var type when type.IsClass && context.Output == null:
                    context.Output = _factory();
                    break;
                default:
                    return;;
            }
        }

        public void Run(ModelMappingContext<T> context)
        {
            InitialiseOutput(context);

            Next();
        }

        public Task Run(ModelMappingContext<T> context, CancellationToken cancellationToken)
        {
            InitialiseOutput(context);

            return Task.CompletedTask;
        }
    }
}