using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.Pipeline.Model
{
    /// <summary>
    ///     If added to a property pipeline Model processors they act like property processors but they don't have access to
    ///     property info like property
    ///     processors. When  part of a Model pipeline the resultant context is set back against the Mapping context Input
    ///     property. This is useful to switch
    ///     context once for all later model & pipeline processors, rather than having to switch context for every property
    ///     pipeline in the schema.
    /// </summary>
    public interface IModelProcessor : IProcessor<ModelContext, ProcessorSchema>
    {
    }
}