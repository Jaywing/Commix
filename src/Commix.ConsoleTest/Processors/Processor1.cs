using System;
using System.Linq;
using Commix.Pipeline.Model;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    public class Processor1 : IModelProcessor
    {
        public Action Next { get; set; }
        public void Run(ModelContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = $"Source was: '{pipelineContext.Context}'";

            Next();
        }
    }

    public class Processor3 : IPropertyProcessor
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = "Processor3 Ran";

            Next();
        }
    }

    public class Processor4 : IPropertyProcessor
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = "Processor4 Ran";

            Next();
        }
    }

    public class Processor5 : IPropertyProcessor
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = "Processor5 Ran";

            Next();
        }
    }

    public class TestModelProcessor : IModelProcessor
    {
        public Action Next { get; set; }

        public void Run(ModelContext pipelineContext, ProcessorSchema processorContext)
        {
            Next();
        }
    }
}