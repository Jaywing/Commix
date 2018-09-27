using System;
using System.Linq;

using Commix.Pipeline.Model;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    public class Processor1 : IContextProcessor
    {
        public Action Next { get; set; }
        public void Run(BasicContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = $"Source was: '{pipelineContext.Context}'";

            Next();
        }
    }

    public class Processor3 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = "Processor3 Ran";

            Next();
        }
    }

    public class Processor4 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = "Processor4 Ran";

            Next();
        }
    }

    public class TestContextProcessor : IContextProcessor
    {
        public Action Next { get; set; }

        public void Run(BasicContext pipelineContext, ProcessorSchema processorContext)
        {
            Next();
        }
    }
}