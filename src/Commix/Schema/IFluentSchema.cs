using System;
using System.Linq;

namespace Commix.Schema
{
    public interface IFluentSchema
    {
        ModelSchemaBuilder Map();
    }
}