using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Commix.Tools;

namespace Commix.Schema
{
    public abstract class SchemaBuilder
    {
        public List<Func<PropertySchema>> PropertyBuilders { get; } = new List<Func<PropertySchema>>();

        protected Type ModelType { get; set; }
        
        public ModelSchema Build()
        {
            var modelSchema = new ModelSchema(ModelType);

            foreach (Func<PropertySchema> propertyBuilder in PropertyBuilders)
                modelSchema.Properties.Add(propertyBuilder());

            return modelSchema;
        }
    }
    
    public class SchemaBuilder<TModel> : SchemaBuilder
    {
        public SchemaBuilder()
        {
            ModelType = typeof(TModel);
        }
        
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