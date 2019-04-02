using System;
using System.Linq;

using Commix.Sitecore.Diagnostics;

namespace Commix.Sitecore
{
    public interface IJsonTraceViewModel
    {
        IJsonTracer Trace { get; set; }
    }
}