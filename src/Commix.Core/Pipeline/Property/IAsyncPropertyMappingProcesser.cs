using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Commix.Core.Pipeline.Property
{
    public interface IAsyncPropertyMappingProcesser<TModel> : IAsyncProcessor<PropertyMappingContext<TModel>>
    {
        Dictionary<string, object> Options { get; set; }
    }
}