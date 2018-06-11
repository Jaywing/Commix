using System;
using System.Linq;
using System.Reflection;

using Commix.Pipeline.Model;

namespace Commix.Pipeline.Property
{
    public class PropertyContext : IMonitoredContext
    {
        public Guid InstanceId { get; } = Guid.NewGuid();

        public ModelContext ModelContext { get; }
        public PropertyInfo PropertyInfo { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PropertyMappingPipeline"/> is faulted.
        /// A faulted pipline will continue to Run whilst Next is called, but once the Pipeline complete if the 
        /// faulted flag is still set the value will not used to set the target property.
        /// </summary>
        /// <value>
        ///   <c>true</c> if aborted; otherwise, <c>false</c>.
        /// </value>
        public bool Faulted { get; set; }

        /// <summary>
        /// Pipeline context, this value will be populated, transformed by the Pipline and then ultimately 
        /// if the Aborted flag is not set  used by a SetProcessor to set the target property.
        /// </summary>
        /// <value>
        /// Pipline context.
        /// </value>
        public object Context { get; set; }

        public PropertyContext(ModelContext modelContext, PropertyInfo propertyInfo)
        {
            ModelContext = modelContext ?? throw new ArgumentNullException(nameof(modelContext));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        public PropertyContext(ModelContext modelContext, PropertyInfo propertyInfo, object initialContext)
            : this(modelContext, propertyInfo)
        {
            Context = initialContext;
        }

        public IPipelineMonitor Monitor { get; set; }
    }
}