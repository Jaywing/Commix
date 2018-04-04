using System;
using System.Linq;

namespace Commix.Core.Schema
{
    public interface IFluentSchema<T>
    {
        void Map(SchemaBuilder<T> schemaBuilder);
    }

    //public interface IAsyncFluentSchema<T>
    //{
    //    void Map(AsyncSchemaBuilder<T> schemaBuilder);
    //}
}