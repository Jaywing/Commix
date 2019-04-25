using System.Reflection;

namespace Commix.Schema
{
    // ReSharper disable UnusedTypeParameter
    public class SchemaPropertyBuilder<TModel, TProp> : SchemaPropertyBuilder
    {
        public SchemaPropertyBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }
    }
}