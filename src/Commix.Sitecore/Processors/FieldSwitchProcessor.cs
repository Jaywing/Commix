using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Exceptions;
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
    public class FieldSwitchProcessor : IBasicProcessor
    {
        public static string FieldId = $"{typeof(FieldSwitchProcessor).Name}FieldId";

        public Action Next { get; set; }

        public void Run(BasicContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    Field contextField = null;

                    switch (pipelineContext.Context)
                    {
                        case Item item:
                            if (processorContext.Options.ContainsKey(FieldId))
                                contextField = item.Fields[processorContext.Options[FieldId].ToString()];
                            else if (pipelineContext is PropertyContext propertyContext)
                                contextField = item.Fields[propertyContext.PropertyInfo.Name];
                            break;
                        case Field field:
                            contextField = field;
                            break;
                        case CustomField customField:
                            contextField = customField.InnerField;
                            break;
                    }

                    switch (contextField)
                    {
                        case null:
                            break;
                        case var _ when string.Equals(contextField.Type, "Checkbox", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new CheckboxField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Date", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Datetime", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new DateField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "File", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new FileField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Image", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new ImageField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Single-Line Text", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Multi-Line Text", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Countable Edit", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new TextField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Rich Text", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new HtmlField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Word Document", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new WordDocumentField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Campaign Tree", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Droptree", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new ReferenceField(contextField);
                            break;

                        case var _ when string.Equals(contextField.Type, "Droplist", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new ValueLookupField(contextField);
                            break;

                        case var _ when string.Equals(contextField.Type, "Grouped Droplink", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new GroupedDroplinkField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Grouped Droplist", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new GroupedDroplistField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Multilist", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Multilist with Search", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Accounts Multilist", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Checklist", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Treelist", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "TreelistEx", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new MultilistField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Name Lookup Value List", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "Name Value List", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new NameValueListField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Droplink", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new LookupField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "General Link", StringComparison.InvariantCultureIgnoreCase):
                        case var _ when string.Equals(contextField.Type, "General Link with Search", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new LinkField(contextField);
                            break;
                        case var _ when string.Equals(contextField.Type, "Version Link", StringComparison.InvariantCultureIgnoreCase):
                            pipelineContext.Context = new VersionLinkField(contextField);
                            break;
                        default:
                            pipelineContext.Context = contextField;
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