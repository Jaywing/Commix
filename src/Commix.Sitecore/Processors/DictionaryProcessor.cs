using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Globalization;

namespace Commix.Sitecore.Processors
{
    public class DictionaryProcessor : IPropertyProcesser
    {
        public static string DictionaryKey = $"{typeof(DictionaryProcessor).Name}Dictionary";
        public static string DefaultValue = $"{typeof(DictionaryProcessor).Name}DefaultValue";
        
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    if (processorContext.TryGetOption(DictionaryKey, out string dictionaryKey))
                    {
                        if (processorContext.TryGetOption(DefaultValue, out string defaultValue) && defaultValue != default(string))
                            pipelineContext.Context = Translate.TextByLanguage(dictionaryKey, Language.Current, defaultValue);
                        else
                            pipelineContext.Context = Translate.Text(dictionaryKey);

                    }
                    else
                    {
                        pipelineContext.Faulted = true;
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