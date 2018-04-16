using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commix.Pipeline
{
    public interface IAsyncProcessor<in T>
    {
        Func<Task> NextAsync { get; set; }

        Task Run(T context, CancellationToken cancellationToken); 
    }
}