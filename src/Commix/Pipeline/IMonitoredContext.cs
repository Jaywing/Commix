namespace Commix.Pipeline
{
    public interface IMonitoredContext
    {
        IPipelineMonitor Monitor { get; set; }
    }
}