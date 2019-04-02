using System;
using Commix.Schema;

namespace Commix.Pipeline.Mapping
{
    /// <summary>
    /// Context used by the model mapping pipeline.
    /// </summary>
    public class MappingContext : IMonitoredContext
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// Mapping source
        /// </summary>
        public object Input { get; set; }

        /// <summary>
        /// Mapping target
        /// </summary>
        public object Output { get; }

        /// <summary>
        /// Schema using for mapping.
        /// </summary>
        public ModelSchema Schema { get; set; }
        
        public MappingContext(object input, object output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public IPipelineMonitor Monitor { get; set; }
    }
}