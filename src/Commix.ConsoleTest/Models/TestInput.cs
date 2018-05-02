using System;
using System.Linq;

namespace Commix.ConsoleTest.Models
{
    public class TestInput
    {
        public string Name { get; set; } = "Hello World";

        public TestInput2 Nested { get; set; } = new TestInput2();
    }
}