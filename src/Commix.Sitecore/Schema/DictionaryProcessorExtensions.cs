using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Pipeline.Property.Processors;
using Commix.Schema;

using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class DictionaryProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Dictionary<TModel, TProp>(
            this SitecoreHelpers<TModel, TProp> builder, string dictionaryKey, 
            string defaultValue = default(string), Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .SchemaBuilder
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