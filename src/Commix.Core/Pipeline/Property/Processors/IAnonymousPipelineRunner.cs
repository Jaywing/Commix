using System;
using System.Linq;

namespace Commix.Core.Pipeline.Property.Processors
{
    public interface IAnonymousPipelineRunner
    {
        object Run(object source);
    }
}
