using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Commix.Core;
using Commix.Core.Pipeline;
using Commix.Core.Pipeline.Model;
using Commix.Core.Pipeline.Model.Processors;
using Commix.Core.Pipeline.Property;
using Commix.Core.Schema;
using Commix.Core.Schema.Extensions;

namespace Commix.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ModelMappingContext<TestOutput>(new TestInput());

            var pipeline = new ModelMappingPipeline<TestOutput>();

            pipeline.Add(new PipelineDiagnosticsProcessor<TestOutput>());
            pipeline.Add(new OutputInitialiseProcessor<TestOutput>());
            pipeline.Add(new SchemaGeneratorProcessor<TestOutput>());
            pipeline.Add(new ModelMapperProcessor<TestOutput>());

            pipeline.Run(context);

            Console.ReadKey();
            Console.WriteLine("Press any key to quit.");
        }
    }


    public class PipelineDiagnosticsProcessor<T> : IProcessor<ModelMappingContext<T>>
    {
        public Action Next { get; set; }
        public void Run(ModelMappingContext<T> context)
        {
            var timer = new Stopwatch();
            timer.Start();
            Next();
            timer.Stop();
            Console.WriteLine($"Pipeline elapsed {timer.Elapsed:G}.");
        }
    }

    public class AsyncPipelineDiagnosticsProcessor<T> : IAsyncProcessor<ModelMappingContext<T>>
    {
        public Func<Task> NextAsync { get; set; }
        public async Task Run(ModelMappingContext<T> context, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();
            await NextAsync();
            timer.Stop();
            Console.WriteLine($"Pipeline elapsed {timer.Elapsed:G}.");
        }
    }

    public class TestInput
    {
        public string SomeString { get; set; } = "Hello World";

        public TestInput2 Nested { get; set; } = new TestInput2();
    }

    public class TestInput2
    {
        public string Name { get; set; } = "Source Nested";
    }

    public class TestOutput : IFluentSchema<TestOutput>
    {
        public string SomeString { get; set; }
        public string SomeDerivedString { get; set; }

        public TestOutput2 Nested { get; set; }

        public void Map(SchemaBuilder<TestOutput> schemaBuilder)
        {
            schemaBuilder
                .Property(m => m.Nested, c => c.Nested())
                .Property(m => m.SomeDerivedString, c => c
                    .From(x => x.SomeString, m => m.Add(Processor.Use<Processor1<TestOutput>>())))
                .Property(m => m.SomeString, c => c.From());
        }
    }

    public class TestOutput2 : IFluentSchema<TestOutput2>
    {
        public string Name { get; set; }
        public void Map(SchemaBuilder<TestOutput2> schemaBuilder)
        {
            schemaBuilder.Property(m => m.Name, c => c.From());
        }
    }

    public class Processor1<T> : IPropertyMappingProcesser<T>
    {
        public Action Next { get; set; }
        public void Run(PropertyMappingContext<T> context)
        {
            context.Value = $"Source was: '{context.Value}'";

            Next();
        }

        public Dictionary<string, object> Options { get; set; }
    }
}