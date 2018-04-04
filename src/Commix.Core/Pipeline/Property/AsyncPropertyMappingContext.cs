using System.Reflection;
using Commix.Core.Pipeline.Model;

namespace Commix.Core.Pipeline.Property
{
    public class PropertyMappingContext<T>
    {
        public ModelMappingContext<T> ModelMappingContext { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public object Value { get; set; }
    }
}