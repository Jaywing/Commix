
using System;

using Commix.Pipeline.Model;
using Commix.Schema;

namespace Commix.Pipeline.Property
{
    public interface IPropertyProcesser : IProcessor<PropertyContext, ProcessorSchema>
    {

    }

    public interface INestedProcessor : IProcessor<NestedContext, ProcessorSchema>
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