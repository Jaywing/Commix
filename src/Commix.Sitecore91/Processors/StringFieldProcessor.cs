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
        public static string RawFieldValue = $"{typeof(StringFieldProcessor).Name}.RawFieldValue";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                var parameters = new StringBuilder();

                if (processorContext.TryGetOption(DisableWebEditingOptionKey, out bool disableWebEditing))
                    parameters.Append($"disable-web-editing={disableWebEditing}");

                if (!pipelineContext.Faulted)
                {
                    switch (pipelineContext.Context)
                    {
                        case TextField field when processorContext.TryGetOption(RawFieldValue, out bool useRawFieldValue) && useRawFieldValue:
                            pipelineContext.Context = field.Value;
                            break;
                        case TextField field:
                            pipelineContext.Context = FieldRenderer.Render(field.InnerField.Item, field.InnerField.ID.ToString(), parameters.ToString());
                            break;
                        case ValueLookupField valueLookupField:
                            // Used for Unbound Droplist
                            pipelineContext.Context = valueLookupField.Value;
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