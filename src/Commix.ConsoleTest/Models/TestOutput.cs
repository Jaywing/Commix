﻿using System;
using System.Collections.Generic;
using System.Linq;

using Commix.ConsoleTest.Processors;
using Commix.Pipeline.Property;
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

        public TestEnum EnumTest { get; set; }

        public SchemaBuilder Map()
            => this.Schema(s => s
                .Model(c => c.Add(Processor.Model<TestModelProcessor>()))
                .Property(m => m.Nested, c => c
                    .Get()
                    .Nested()
                    .Set())
                .Property(m => m.SomeDerivedString, c => c
                    .Get("Name")
                    .Add(Processor.Model<Processor1>())
                    .Set())
                .Property(m => m.Name, c => c
                    .Get()
                    .Set())
                .Property(m => m.Col, c => c
                    .Constant(new List<TestInput>{new TestInput(), new TestInput()})
                    .Collection(x => x.Define<TestInput, TestInput2>())
                    .Ensure(new List<TestInput2>{new TestInput2(){Name = "switched"}})
                    .Set())
                .Property(m => m.EnumTest, p => p
                    .Get()
                    .Add(Processor.Property<EnumProcessor<TestEnum>>())
                    .Set())
            );


        public class TestOutput2 : IFluentSchema
        {
            public string Name { get; set; }

            public string StageResult { get; set; }

            public SchemaBuilder Map()
                => this.Schema(s => s
                    .Property(m => m.Name, c => c.Get().Set())
                    .Property(m => m.StageResult, p => p
                        .SetStage(PropertyStageMarker.Populating)
                        .Add(Processor.Property<Processor3>())
                        .Add(Processor.Property<Processor4>())
                        .SetStage(PropertyStageMarker.Finalised, c => c.Option(SetStageProcessor.TypeCheck, typeof(string)))
                        .Add(Processor.Property<Processor5>(o => o
                            .AllowedStages(PropertyStageMarker.Populating)
                        ))
                        .Set())
                );
        }
    }
}