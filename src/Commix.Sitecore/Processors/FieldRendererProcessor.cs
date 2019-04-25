using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;

namespace Commix.Sitecore.Processors
{
    public class FieldRendererProcessor : IPropertyProcessor
    {
        public static readonly string CssClass = $"{typeof(FieldRendererProcessor).Name}CssClass";
        public static readonly string Width = $"{typeof(FieldRendererProcessor).Name}Width";
        public static readonly string Height = $"{typeof(FieldRendererProcessor).Name}Height";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                string parameters = string.Empty;

                if (processorContext.Options.ContainsKey(CssClass))
                {
                    parameters = AddParameter(parameters, $"class={processorContext.Options[CssClass]}");
                }

                if (processorContext.Options.ContainsKey(Width) 
                    && int.TryParse(processorContext.Options[Width].ToString(), out int width))
                {
                    parameters = AddParameter(parameters, $"w={width}");
                }

                if (processorContext.Options.ContainsKey(Height)
                    && int.TryParse(processorContext.Options[Height].ToString(), out int height))
                {
                    parameters = AddParameter(parameters, $"h={height}");
                }

                switch (pipelineContext.Context)
                {
                    case CustomField customField:
                        pipelineContext.Context = FieldRenderer.Render(customField.InnerField.Item, customField.InnerField.ID.ToString(), parameters);
                        break;
                    default:
                        pipelineContext.Faulted = true;
                        break;
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

        private static string AddParameter(string parameters, string parameter)
        {
            return string.IsNullOrWhiteSpace(parameters) ? parameter : $"{parameters}&{parameter}";
        }
    }
}
