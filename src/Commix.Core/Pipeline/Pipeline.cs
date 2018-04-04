using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Core.Pipeline
{
    public class Pipeline<T>
    {
        private readonly List<IProcessor<T>> _processors = new List<IProcessor<T>>();

        public void Add(IProcessor<T> processor) => _processors.Add(processor);

        public void Run(T context)
        {
            if (_processors == null || _processors.Count == 0)
                return;

            for (int i = 0; i < _processors.Count; i++)
            {
                var stepIndex = i;
                _processors[i].Next = () =>
                {
                    if (stepIndex + 1 < _processors.Count)
                        _processors[stepIndex + 1].Run(context);
                };
            }

            _processors[0].Run(context);
        }
    }
}