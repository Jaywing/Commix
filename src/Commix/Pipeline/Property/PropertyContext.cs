using System;
using System.Reflection;
using Commix.Pipeline.Mapping;

namespace Commix.Pipeline.Property
{
    /// <summary>
    ///     Property contexts are passed to IPropertyProcessors, used to build up a context with the aim to set a property on
    ///     the MappingContext output.
    /// </summary>
    public class PropertyContext : ModelContext
    {
        public PropertyInfo PropertyInfo { get; }

        public PropertyContext(MappingContext mappingContext, PropertyInfo propertyInfo, object initialContext)
            : base(mappingContext, initialContext) =>
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
    }
}