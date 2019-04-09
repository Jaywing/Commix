using System;
using System.Collections.Generic;

namespace Commix.Schema
{
    public abstract class ModelSchemaBuilder
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
}