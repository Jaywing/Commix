using System;
using System.Linq;

using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class CheckboxFieldProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> CheckboxField<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string fieldId)
        {
            return builder
                .Add(Processor.Use<FieldSwitchProcessor>(c => c
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Use<CheckboxFieldProcessor>());
        }
    }
}