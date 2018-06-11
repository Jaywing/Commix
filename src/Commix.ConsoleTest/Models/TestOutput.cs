using System;
using System.Collections.Generic;
using System.Linq;

using Commix.ConsoleTest.Processors;
using Commix.Pipeline.Property.Processors;

using Commix.Schema;

namespace Commix.ConsoleTest.Models
{

    public class SimpleTestOutput : IFluentSchema
    {
        public string Name { get; set; }

        public SchemaBuilder Map()
            => this.Schema(s => s
                .Property(m => m.Name, p => p
                    .Constant("Hello")
                    .Set())
            );
    }

    public class TestOutput : IFluentSchema
    {
        public string Name { get; set; }
        public string SomeDerivedString { get; set; }

        public TestOutput2 Nested { get; set; }

        public IEnumerable<TestInput2> Col { get; set; }

        public SchemaBuilder Map()
            => this.Schema(s => s
                .Property(m => m.Nested, c => c
                    .NestedFrom()
                    .Set())
                .Property(m => m.SomeDerivedString, c => c
                    .Get("Name")
                    .Add(Processor.Use<Processor1>())
                    .Set())
                .Property(m => m.Name, c => c
                    .Get()
                    .Set())
                .Property(m => m.Col, c => c
                    .Constant(new List<TestInput>{new TestInput(), new TestInput()})
                    .Collection(x => x.Define<TestInput, TestInput2>())
                    .Ensure(typeof(IEnumerable<TestInput2>), new List<TestInput2>{new TestInput2(){Name = "switched"}})
                    .Set())
            );


        public class TestOutput2 : IFluentSchema
        {
            public string Name { get; set; }

            public TestOutput3 Test3 { get; set; }

            public SchemaBuilder Map()
                => this.Schema(s => s
                    .Property(m => m.Name, c => c.Get())
                    .Property(m => m.Test3, p => p
                        .Add(Processor.Use<Processor2>())
                        .Set())
                );
        }
    }
}