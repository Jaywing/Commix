using System;
using Commix.Pipeline.Mapping;

namespace Commix.Pipeline.Property
{
    /// <summary>
    ///     Model contexts are passed to IModelProcessors, used to change the value of MappingContext input as part of an
    ///     on-going mapping operation.
    /// </summary>
    public class ModelContext : IMonitoredContext
    {
        public PropertyStageMarker Stage { get; set; }

        public Guid InstanceId { get; } = Guid.NewGuid();

        public MappingContext MappingContext { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="PropertyPipeline" /> is faulted.
        ///     A faulted pipline will continue to Run whilst Next is called, but once the Pipeline complete if the
        ///     faulted flag is still set the value will not used to set the target property.
        /// </summary>
        /// <value>
        ///     <c>true</c> if aborted; otherwise, <c>false</c>.
        /// </value>
        public bool Faulted { get; set; }

        /// <summary>
        ///     Pipeline context, this value will be populated, transformed by the Pipline and then ultimately
        ///     if the Aborted flag is not set used by a SetProcessor to set the target property.
        /// </summary>
        /// <value>
        ///     Pipline context.
        /// </value>
        public object Context { get; set; }

        public IPipelineMonitor Monitor { get; set; }

        public ModelContext(MappingContext mappingContext, object initialContext)
        {
            MappingContext = mappingContext;
            Context = initialContext;
        }
    }
}