using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Commix.Core.Pipeline
{
    public class AsyncPipeline<T>
    {
        private readonly List<IAsyncProcessor<T>> _processors = new List<IAsyncProcessor<T>>();

        public void Add(IAsyncProcessor<T> processor) => _processors.Add(processor);

        public async Task Run(T context, CancellationToken cancellationToken)
        {
            if (_processors == null || _processors.Count == 0)
                return;

            for (int i = 0; i < _processors.Count; i++)
            {
                var stepIndex = i;
                _processors[i].NextAsync = async () =>
                {
                    if (!cancellationToken.IsCancellationRequested && stepIndex + 1 < _processors.Count)
                        await _processors[stepIndex + 1].Run(context, cancellationToken);
                };
            }

            await _processors[0].Run(context, cancellationToken);
        }
    }
}