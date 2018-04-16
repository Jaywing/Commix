using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Commix.Tools;

namespace Commix.Schema
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

    public static class FluentSchemaExtensions
    {
        public static SchemaBuilder Schema<T>(this T model, Action<SchemaBuilder<T>> configure)
            where T : IFluentSchema
        {
            var builder = new SchemaBuilder<T>();
            configure(builder);
            return builder;
        }
    }
}