using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commix.Core.Pipeline
{
    public class AsyncProcessorWrapper<T> : IAsyncProcessor<T>
    {
        private readonly IProcessor<T> _processor;

        public AsyncProcessorWrapper(IProcessor<T> processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public Func<Task> NextAsync { get; set; }

        public Task Run(T context, CancellationToken cancellationToken)
        {
            _processor.Next = () =>
            {
                if (!cancellationToken.IsCancellationRequested)
                    NextAsync();
            };

            if (!cancellationToken.IsCancellationRequested)
                _processor.Run(context);

            return Task.CompletedTask;
        }
    }
}
