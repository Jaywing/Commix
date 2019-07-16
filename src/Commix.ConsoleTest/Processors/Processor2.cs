using System;
using System.Linq;

using Commix.ConsoleTest.Models;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.ConsoleTest.Processors
{
    public class Processor2 : IPropertyProcessor
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            pipelineContext.Context = new TestOutput3().As<TestOutput3>();

            Next();
        }
    }
}

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{}