using System;
using System.Collections.Generic;
using System.Linq;

namespace Commix.Schema
{
    public class ModelSchema
    {
        public Type ModelType { get; }

        public List<PipelineSchema> Schemas { get; } = new List<PipelineSchema>();

        public ModelSchema(Type modelType)
        {
            ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
        }
    }
}