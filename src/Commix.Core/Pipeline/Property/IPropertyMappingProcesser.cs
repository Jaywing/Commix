using System.Collections.Generic;

namespace Commix.Core.Pipeline.Property
{
    public interface IPropertyMappingProcesser<TModel> : IProcessor<PropertyMappingContext<TModel>>
    {
        Dictionary<string, object> Options { get; set; }
    }
}