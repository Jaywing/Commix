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
            if (!(pipelineContext.Context is Field field))
            {
                if (pipelineContext.Context is Item item)
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
                        pipelineContext.Context = new CheckboxField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Date", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Datetime", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new DateField(field);
                        break;
                    case var _ when string.Equals(field.Type, "File", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new FileField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Image", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new ImageField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Single-Line Text", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Multi-Line Text", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Countable Edit", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new TextField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Rich Text", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new HtmlField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Word Document", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new WordDocumentField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Campaign Tree", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Droptree", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new ReferenceField(field);
                        break;

                    case var _ when string.Equals(field.Type, "Droplist", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new ValueLookupField(field);
                        break;

                    case var _ when string.Equals(field.Type, "Grouped Droplink", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new GroupedDroplinkField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Grouped Droplist", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new GroupedDroplistField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Multilist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Multilist with Search", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Accounts Multilist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Checklist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Treelist", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "TreelistEx", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new MultilistField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Name Lookup Value List", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "Name Value List", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new NameValueListField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Droplink", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new LookupField(field);
                        break;
                    case var _ when string.Equals(field.Type, "General Link", StringComparison.InvariantCultureIgnoreCase):
                    case var _ when string.Equals(field.Type, "General Link with Search", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new LinkField(field);
                        break;
                    case var _ when string.Equals(field.Type, "Version Link", StringComparison.InvariantCultureIgnoreCase):
                        pipelineContext.Context = new VersionLinkField(field);
                        break;
                    default:
                        pipelineContext.Context = field;
                        break;
                }
            }

            Next();
        }
    }
}