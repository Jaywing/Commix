using System;

namespace Commix.Core.Pipeline
{
    public interface IProcessor<in T>
    {
        Action Next { get; set; }

        void Run(T context);
    }
}