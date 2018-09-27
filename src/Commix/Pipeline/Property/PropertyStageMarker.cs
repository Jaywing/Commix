using System;
using System.Linq;

namespace Commix.Pipeline.Property
{
    /// <summary>
    /// Stage margers control at which point of a pipeline processors are allowed to run,
    /// classic use is to set a pipeline to finalised after a cache hit, to skip population steps.
    /// </summary>
    [Flags]
    public enum PropertyStageMarker : uint
    {
        Populating = 0,
        Finalised = 1,
        All = ~0u
    }
}