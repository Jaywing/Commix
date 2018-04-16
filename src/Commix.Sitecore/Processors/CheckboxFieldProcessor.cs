using System;
using System.Linq;

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
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            switch (pipelineContext.Value)
            {
                case Field field when field.Type == "checkbox":
                    CheckboxField checkBoxField = field;
                    pipelineContext.Value = checkBoxField.Checked;
                    break;
            }
            
            Next();
        }
    }
}