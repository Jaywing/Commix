using System;
using System.Linq;
using System.Reflection;

using Commix.Pipeline.Model;

namespace Commix.Pipeline.Property
{
    public class PropertyContext
    {
        public ModelContext ModelContext { get; }
        public PropertyInfo PropertyInfo { get; }

        public object Value { get; set; }

        public PropertyContext(ModelContext modelContext, PropertyInfo propertyInfo)
        {
            ModelContext = modelContext ?? throw new ArgumentNullException(nameof(modelContext));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
    }
}