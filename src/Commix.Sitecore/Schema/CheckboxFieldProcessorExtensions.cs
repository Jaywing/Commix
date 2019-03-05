using System;
using System.Linq;

using Commix.Pipeline.Property;
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
                .Add(Processor.Model<FieldSwitchProcessor>(c => c
                    .AllowedStages(PropertyStageMarker.Populating)
                    .Option(FieldSwitchProcessor.FieldId, fieldId)))
                .Add(Processor.Property<CheckboxFieldProcessor>(o => o
                    .AllowedStages(PropertyStageMarker.Populating)
                ));
        }
    }
}