using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    public class Processor1 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            pipelineContext.Context = $"Source was: '{pipelineContext.Context}'";

            Next();
        }
    }
}