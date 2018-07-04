using System;
using System.Linq;

namespace Commix.Pipeline
{
    public interface IMonitoredContext
    {
        IPipelineMonitor Monitor { get; set; }
    }
}