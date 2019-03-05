using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// If added to a property pipeline Model processors act like property processors but they do not have access to property info like property processors. When
    /// part of a Model pipeline the resultant context is set back againt the Mapping context Input property. This is usefull to switch context once
    /// for all later pipeline processors, rather than having to switch context for every property pipeline in the model schema.
    /// </summary>
    public interface IModelProcessor : IProcessor<ModelContext, ProcessorSchema>
    {

    }
}