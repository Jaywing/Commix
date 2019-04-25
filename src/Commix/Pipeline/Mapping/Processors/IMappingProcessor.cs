namespace Commix.Pipeline.Mapping.Processors
{
    /// <summary>
    ///     Model pipeline processor, uses the pipeline schema to map a model.
    /// </summary>
    public interface IMappingProcessor : IProcessor<MappingContext, MappingProcessorContext>
    {
    }
}