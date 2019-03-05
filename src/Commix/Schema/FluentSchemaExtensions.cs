using System;

namespace Commix.Schema
{
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