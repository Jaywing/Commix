using System;
using System.Linq;

using Commix.Pipeline.Property;

namespace Commix.Pipeline.Model
{
    public interface IPropertyProcessorFactory
    {
        IPropertyProcesser GetProcessor(Type processorType);
    }
}