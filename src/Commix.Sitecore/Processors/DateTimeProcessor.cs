using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;

using SitecoreContext = Sitecore.Context;

namespace Commix.Sitecore.Processors
{
    public class DateTimeProcessor : IPropertyProcessor
    {
        public static readonly string DateTimeFormat = $"{typeof(DateTimeProcessor).Name}DateTimeFormat";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                string dateTimeFormat = string.Empty;

                if (processorContext.Options.ContainsKey(DateTimeFormat))
                {
                    dateTimeFormat = processorContext.Options[DateTimeFormat].ToString();
                }

                switch (pipelineContext.Context)
                {
                    case DateField field when !SitecoreContext.PageMode.IsExperienceEditor:
                        pipelineContext.Context = string.IsNullOrWhiteSpace(field.Value) ? string.Empty : field.DateTime.ToString(dateTimeFormat);
                        break;
                    case DateField field:
                        pipelineContext.Context =
                            FieldRenderer.Render(field.InnerField.Item, field.InnerField.ID.ToString());
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
    }
}
