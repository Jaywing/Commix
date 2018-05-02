using System;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;

namespace Commix.ConsoleTest
{
    public class TestModelMappingPipeline : ModelMappingPipeline
    {
        public TestModelMappingPipeline(SchemaGeneratorProcessor schemaGenerator, IModelMapperProcessor modelMapper)
        {
            Add(schemaGenerator, new ModelProcessorContext());
            Add(modelMapper, new ModelProcessorContext());
        }
    }
}