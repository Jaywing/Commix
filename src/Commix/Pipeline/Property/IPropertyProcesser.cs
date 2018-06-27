
using System;

using Commix.Schema;

namespace Commix.Pipeline.Property
{
    public interface IPropertyProcesser : IProcessor<PropertyContext, PropertyProcessorSchema>
    {

    }

    [Flags]
    public enum PropertyStageMarker : uint
    {
        Populating = 0,
        Finalised = 1,
        All = ~0u
    }
}