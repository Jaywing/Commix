using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;
using Commix.Schema;
using Commix.Sitecore.Processors;

namespace Commix.Sitecore.Schema
{
    public static class DictionaryProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Dictionary<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string dictionaryKey, string defaultValue)
        {
            return builder
                .Add(Processor.Use<DictionaryProcessor>(c => c
                    .Option(DictionaryProcessor.DictionaryKeyOptionKey, dictionaryKey)))
                .Ensure(typeof(string), defaultValue);
        }
    }
}