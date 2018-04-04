using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Commix.Core.Tools;

namespace Commix.Core.Schema
{
    public abstract class SchemaBuilder
    {
        protected readonly List<Func<PropertySchema>> PropertyBuilders = new List<Func<PropertySchema>>();
        
        public ModelSchema Build()
        {
            var modelSchema = new ModelSchema();

            foreach (Func<PropertySchema> propertyBuilder in PropertyBuilders)
            {
                var propertySchema = propertyBuilder();
                modelSchema.Properties.Add(propertySchema);
            }

            return modelSchema;
        }
    }
    
    
    public class SchemaBuilder<TModel> : SchemaBuilder
    {
        public SchemaBuilder<TModel> Property<TProp>(Expression<Func<TModel, TProp>> property,
            Action<SchemaPropertyBuilder<TModel, TProp>> configure)
        {
            var propertyInfo = PropertyHelper<TModel>.GetProperty(property);
            var propertyBuilder = new SchemaPropertyBuilder<TModel, TProp>(propertyInfo);

            configure(propertyBuilder);

            PropertyBuilders.Add(propertyBuilder.Build);

            return this;
        }
    }

    //public class AsyncSchemaBuilder<TModel> : SchemaBuilder
    //{
    //    public AsyncSchemaBuilder<TModel> Property<TProp>(
    //        Expression<Func<TModel, TProp>> property,
    //        Action<AsyncSchemaPropertyBuilder<TModel, TProp>> configure)
    //    {
    //        var propertyInfo = PropertyHelper<TModel>.GetProperty(property);
    //        var propertyBuilder = new AsyncSchemaPropertyBuilder<TModel, TProp>(propertyInfo);

    //        configure(propertyBuilder);

    //        PropertyBuilders.Add(propertyBuilder.Build);

    //        return this;
    //    }
    //}
}