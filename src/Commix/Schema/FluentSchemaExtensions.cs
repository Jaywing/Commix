using System;

namespace Commix.Schema
{
    public static class FluentSchemaExtensions
    {
        public static ModelSchemaBuilder Schema<T>(this T model, Action<ModelSchemaBuilder<T>> configure)
            where T : IFluentSchema
        {
            var builder = new ModelSchemaBuilder<T>();
            configure(builder);
            return builder;
        }
    }
}