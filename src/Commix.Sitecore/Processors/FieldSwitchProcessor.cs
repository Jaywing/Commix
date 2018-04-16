using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Pipeline;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Switch the context to a field on a <see cref="Item"/>, expects the context to be a <see cref="Item"/>.
    /// </summary>
    /// <seealso cref="Commix.Pipeline.Property.IPropertyProcesser" />
    public class FieldSwitchProcessor : IPropertyProcesser
    {
        public static string FieldId = $"{typeof(FieldSwitchProcessor).Name}FieldId";

        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (!(pipelineContext.Value is Field field))
            {
                if (pipelineContext.Value is Item item)
                {
                    string fieldId;
                    if (processorContext.Options.ContainsKey(FieldId))
                        fieldId = processorContext.Options[FieldId].ToString();
                    else
                        fieldId = pipelineContext.PropertyInfo.Name;

                    field = item.Fields[fieldId];
                }
                else
                {
                    Next();
                    return;
                }
            }

            if (field != null)
            {
                switch (field)
                {
                    case var _ when string.Equals(field.Type, "Checkbox", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new CheckboxField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Date", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Datetime", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new DateField(field);
                        break;
                    case var _ when string.Equals(field.Type, "File", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new FileField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Image", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new ImageField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Single-Line Text", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Multi-Line Text", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Countable Edit", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new TextField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Rich Text", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new HtmlField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Word Document", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new WordDocumentField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Campaign Tree", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Droptree", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new ReferenceField(field);
                        break;

                    case var _ when string.Equals(field.Type, "Droplist", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new ValueLookupField(field);
                        break;

                    case var _ when string.Equals(field.Type, "Grouped Droplink", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new GroupedDroplinkField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Grouped Droplist", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new GroupedDroplistField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Multilist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Multilist with Search", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Accounts Multilist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Checklist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Treelist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "TreelistEx", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new MultilistField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Name Lookup Value List", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Name Value List", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new NameValueListField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Droplink", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new LookupField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Droplink", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new LookupField(field);
                        break;
                    case var _ when string.Equals(field.Type, "General Link", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "General Link with Search", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new LinkField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Version Link", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Value = new VersionLinkField(field);
                        break;
                    default:
                        pipelineContext.Value = field;
                        break;
                }
            }

            Next();
        }
    }
}