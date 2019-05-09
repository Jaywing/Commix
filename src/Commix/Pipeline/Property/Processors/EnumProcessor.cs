using System;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Property.Processors
{
    public class EnumProcessor<T> : IPropertyProcessor where T : struct
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                switch (pipelineContext.Context)
                {
                    case string stringValue when Enum.TryParse(stringValue, true, out T value):
                        pipelineContext.Context = value;
                        break;
                    case string stringValue when TryMatchByAttribute(stringValue, out T value):
                        pipelineContext.Context = value;
                        break;
                    default:
                        pipelineContext.Faulted = true;
                        break;
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

        private bool TryMatchByAttribute(string value, out T match)
        {
            foreach (var enumValue in Enum.GetValues(typeof(T)))
            {
                var enumName = Enum.GetName(typeof(T), enumValue);
                var aliasAttribute = typeof(T).GetField(enumName)
                    .GetCustomAttributes(false)
                    .OfType<EnumAliasAttribute>()
                    .FirstOrDefault();

                if (aliasAttribute != null)
                {
                    if (string.Equals(aliasAttribute.Alias, value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        match = (T) enumValue;
                        return true;
                    }
                }
            }

            match = default(T);
            return false;
        }
    }

    public class EnumAliasAttribute : Attribute
    {
        public string Alias { get; set; }

        public EnumAliasAttribute(string alias)
        {
            Alias = alias ?? throw new ArgumentNullException(nameof(alias));
        }
    }
}