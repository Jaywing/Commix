using System;
using System.Collections.Generic;

namespace Commix.Schema
{
    public class ModelSchema
    {
        public Type ModelType { get; }

        public List<IPipelineSchema> Schemas { get; } = new List<IPipelineSchema>();

        public ModelSchema(Type modelType) => ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
    }
}