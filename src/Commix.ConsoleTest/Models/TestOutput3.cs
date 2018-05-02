﻿using System;
using System.Linq;

using Commix.Schema;
using Commix.Schema.Extensions;

namespace Commix.ConsoleTest.Models
{
    public class TestOutput3 : IFluentSchema
    {
        public int Prop3 { get; set; }
        
        public SchemaBuilder Map()
            => this.Schema(s => s
                .Property(m => m.Prop3, p => p
                    .ConstantValue(9001)
                    .Set())
            );
    }
}