using System;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Model
{
    /// <summary>
    /// Context used by the model mapping pipeline.
    /// </summary>
    public class ModelContext : IMonitoredContext
    {
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
        
        public ModelContext(object input, object output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public IPipelineMonitor Monitor { get; set; }
    }
}