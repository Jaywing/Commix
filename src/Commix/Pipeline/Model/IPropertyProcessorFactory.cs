using System;
using Commix.Pipeline.Property;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// Get a processor by type.
    /// </summary>
    public interface IPropertyProcessorFactory
    {
        IPropertyProcesser GetProcessor(Type processorType);
    }
}