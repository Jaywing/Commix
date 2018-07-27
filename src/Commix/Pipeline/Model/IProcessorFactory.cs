using System;
using Commix.Pipeline.Property;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// Get a processor by type.
    /// </summary>
    public interface IProcessorFactory
    {
        bool TryGetProcessor<T>(Type processorType, out T propertyProcessor)  where T : class;
    }
}