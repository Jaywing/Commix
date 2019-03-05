using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Commix.Schema
{
    // ReSharper disable UnusedTypeParameter
    public class SchemaPropertyBuilder<TModel, TProp> : SchemaPropertyBuilder
    {
        public SchemaPropertyBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        { }
    }
}
