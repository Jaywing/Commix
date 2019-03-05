using System;

namespace Commix.Pipeline
{
    /// <summary>
    /// Get a processor by type.
    /// </summary>
    public interface IProcessorFactory
    {
        bool TryGetProcessor<T>(Type processorType, out T propertyProcessor)  where T : class;
    }
}