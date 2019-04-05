using System;
using System.Linq;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;

namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Set the context to the value of a <see cref="CheckboxField"/>, expects the context to be a <see cref="CheckboxField"/>.
    /// </summary>
    /// <seealso cref="Commix.Pipeline.Property.IPropertyProcesser" />
    public class CheckboxFieldProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    switch (pipelineContext.Context)
                    {
                        case CheckboxField checkboxField:
                            pipelineContext.Context = checkboxField.Checked;
                            break;
                        case Field field when string.Equals(field.Type, "Checkbox", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = ((CheckboxField)field).Checked;
                            break;
                        default:
                            pipelineContext.Faulted = true;
                            break;
                    }
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }
            finally
            {
                Next();
            }
        }
    }
}