using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;

namespace Commix.Sitecore.Processors
{
    public class StringFieldProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (pipelineContext.Value is TextField field)
            {
                pipelineContext.Value = FieldRenderer.Render(field.InnerField.Item, field.InnerField.ID.ToString());
            }

            Next();
        }
    }
}