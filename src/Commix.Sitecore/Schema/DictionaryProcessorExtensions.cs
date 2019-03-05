using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Schema.Extensions;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class DictionaryProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Dictionary<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string dictionaryKey, 
            string defaultValue = default(string), Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<DictionaryProcessor>(c =>
                {
                    c.AllowedStages(PropertyStageMarker.Populating);
                    c.Option(DictionaryProcessor.DictionaryKey, dictionaryKey);
                    c.Option(DictionaryProcessor.DefaultValue, defaultValue);
                    configure?.Invoke(c);;
                }))
                .Ensure(defaultValue);
        }
    }
}