using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Schema.Extensions;

using Sitecore.Globalization;

namespace Commix.Sitecore.Processors
{
    public class DictionaryProcessor : IPropertyProcesser
    {
        public static string DictionaryKeyOptionKey = $"{typeof(DictionaryProcessor).Name}DictionaryKey";
        
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            if (processorContext.TryGetOption(DictionaryKeyOptionKey, out string dictionaryKey))
                pipelineContext.Context = Translate.Text(dictionaryKey);

            Next();
        }
    }

    public static partial class FieldProcessorExtensions
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