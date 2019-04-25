using System;
using System.Collections.Generic;

namespace Commix.Schema
{
    public abstract class SchemaBuilder
    {
        internal List<Func<IPipelineSchema>> SchemaBuilders { get; } = new List<Func<IPipelineSchema>>();

        protected Type ModelType { get; set; }

        public ModelSchema Build()
        {
            var modelSchema = new ModelSchema(ModelType);

            foreach (Func<IPipelineSchema> schemaBuilder in SchemaBuilders) modelSchema.Schemas.Add(schemaBuilder());

            return modelSchema;
        }
    }
}