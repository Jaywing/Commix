using System;
using System.Linq;

using Commix.Pipeline.Property.Processors;

namespace Commix.ConsoleTest.Models
{
    public class TestInput
    {
        public string Name { get; set; } = "Hello World";

        public TestInput2 Nested { get; set; } = new TestInput2();

        public string EnumTest { get; set; } = "gamma-alias";
    }

    public enum TestEnum
    {
        Alpha,
        Beta,
        [EnumAlias("gamma-alias")]
        Gamma
    }
}