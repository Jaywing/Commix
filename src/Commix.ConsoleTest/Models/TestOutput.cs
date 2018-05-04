using System;
using System.Collections.Generic;
using System.Linq;

using Commix.ConsoleTest.Processors;
using Commix.Schema;
using Commix.Schema.Extensions;

namespace Commix.ConsoleTest.Models
{
    public class TestOutput : IFluentSchema
    {
        public string Name { get; set; }
        public string SomeDerivedString { get; set; }

        public TestOutput2 Nested { get; set; }

        public TestOutput2 Col { get; set; }

        public SchemaBuilder Map()
            => this.Schema(s => s
                .Property(m => m.Nested, c => c.NestedFrom())
                .Property(m => m.SomeDerivedString, c => c
                    .Get("Name")
                    .Add(Processor.Use<Processor1>()))
                .Property(m => m.Name, c => c.Get())
                .Property(m => m.Col, c => c
                    .ConstantValue("Hello")
                    .Ensure(typeof(TestOutput2), new TestOutput2(){Name = "switched"})
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