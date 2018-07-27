using System;
using System.Linq;

using Commix.ConsoleTest.Models;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    public class Processor2 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = new TestOutput3().As<TestOutput3>();

            Next();
        }
    }
}