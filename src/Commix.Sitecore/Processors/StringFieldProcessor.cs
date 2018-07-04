using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;

namespace Commix.Sitecore.Processors
{
    public class StringFieldProcessor : IPropertyProcesser
    {
        public static string DisableWebEditingOptionKey = $"{typeof(StringFieldProcessor).Name}.DisableWebEditing"; 
        
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (pipelineContext.Context is TextField field)
                    {
                        var parameters = new StringBuilder();

                        if (processorContext.TryGetOption(DisableWebEditingOptionKey, out bool disableWebEditing))
                            parameters.Append($"disable-web-editing={disableWebEditing}");

                        pipelineContext.Context = FieldRenderer.Render(field.InnerField.Item, field.InnerField.ID.ToString(), parameters.ToString());
                    }
                    else
                    {
                        pipelineContext.Faulted = true;
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