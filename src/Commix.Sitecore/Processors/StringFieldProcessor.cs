using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Schema.Extensions;

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
            if (pipelineContext.Context is TextField field)
            {
                var parameters = new StringBuilder();

                if (processorContext.TryGetOption(DisableWebEditingOptionKey, out bool disableWebEditing))
                    parameters.Append($"disable-web-editing={disableWebEditing}");

                pipelineContext.Context = FieldRenderer.Render(field.InnerField.Item, field.InnerField.ID.ToString(), parameters.ToString());
            }

            Next();
        }
    }

    public static partial class FieldProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return StringField(builder, fieldId, string.Empty, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId, string defaultValue)
        {
            return StringField(builder, fieldId, defaultValue, false);
        }

        public static SchemaPropertyBuilder<TModel, TProp> StringField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId, string defaultValue, bool disableWebEditing)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<StringFieldProcessor>(c => c
                    .Option(StringFieldProcessor.DisableWebEditingOptionKey, disableWebEditing)))
                .Ensure(typeof(string), defaultValue);
        }
    }
}