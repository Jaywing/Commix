using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commix.Core.Pipeline
{
    public interface IAsyncProcessor<in T>
    {
        Func<Task> NextAsync { get; set; }

        Task Run(T context, CancellationToken cancellationToken); 
    }
}