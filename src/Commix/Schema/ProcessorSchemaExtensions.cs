using System;

namespace Commix.Schema
{
    public static class ProcessorSchemaExtensions
    {
        public static bool TryGetOption<T>(this ProcessorSchema schema, string key, out T value)
        {
            if (schema.Options == null || !schema.Options.ContainsKey(key))
            {
                value = default(T);
                return false;
            }

            try
            {
                value = (T) schema.Options[key];
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }
    }
}