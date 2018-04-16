using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Switch the context to a <see cref="ID"/>, expects the context to be a <see cref="Field"/> with a value parseable as an <see cref="ID"/>.
    /// </summary>
    /// <seealso cref="Commix.Pipeline.Property.IPropertyProcesser" />
    public class IdFieldProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (!(pipelineContext.Value is Field field))
                throw new InvalidOperationException();

            ID value;
            if (!ID.TryParse(field.GetValue(true), out value))
                throw new InvalidOperationException();
            
            pipelineContext.Value = value;
            
            Next();
        }
    }
}