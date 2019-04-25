using System;
using System.Linq.Expressions;
using Commix.Tools;

namespace Commix.Schema
{
    public class SchemaBuilder<TModel> : SchemaBuilder
    {
        public SchemaBuilder() => ModelType = typeof(TModel);

        public SchemaBuilder<TModel> Property<TProp>(Expression<Func<TModel, TProp>> property,
            Action<SchemaPropertyBuilder<TModel, TProp>> configure)
        {
            var propertyInfo = PropertyHelper<TModel>.GetProperty(property);
            var propertyBuilder = new SchemaPropertyBuilder<TModel, TProp>(propertyInfo);

            configure(propertyBuilder);

            SchemaBuilders.Add(propertyBuilder.Build);

            return this;
        }

        public SchemaBuilder<TModel> Model(Action<SchemaModelBuilder<TModel>> configure)
        {
            var propertyBuilder = new SchemaModelBuilder<TModel>();

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
}