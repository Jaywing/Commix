using System;
using System.Linq;

using Commix.Pipeline;

namespace Commix.Sitecore.Diagnostics
{
    public interface IJsonTracer : IDisposable
    {
        string ToJson();
        void Attach(IPipelineMonitor monitor);
    }
}