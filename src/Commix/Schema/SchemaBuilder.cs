using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Commix.Tools;

namespace Commix.Schema
{
    public abstract class SchemaBuilder
    {
        internal List<Func<PipelineSchema>> SchemaBuilders { get; } = new List<Func<PipelineSchema>>();

        protected Type ModelType { get; set; }
        
        public ModelSchema Build()
        {
            var modelSchema = new ModelSchema(ModelType);

            foreach (Func<PipelineSchema> schemaBuilder in SchemaBuilders)
            {
                modelSchema.Schemas.Add(schemaBuilder());
            }

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

            SchemaBuilders.Add(propertyBuilder.Build);

            return this;
        }

        public SchemaBuilder<TModel> Context(Action<SchemaContextBuilder<TModel>> configure)
        {
            var propertyBuilder = new SchemaContextBuilder<TModel>();

            configure(propertyBuilder);

            SchemaBuilders.Add(propertyBuilder.Build);

            return this;
        }

        public SchemaBuilder<TModel> Merge(SchemaBuilder builder)
        {
            SchemaBuilders.AddRange(builder.SchemaBuilders);

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