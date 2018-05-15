using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commix;
using Commix.ConsoleTest.Models;
using Commix.Core;
using Commix.Pipeline;
using Commix.Pipeline.Model;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Type typeToEnsure = typeof(IEnumerable<TestInput2>);
            
            var sourceType = new List<Object>().GetType();

            var replacementtype = new List<TestInput2>().GetType();

            if (typeToEnsure.IsAssignableFrom(replacementtype) &&
                !sourceType.IsAssignableFrom(typeToEnsure))
            {

            }
            
            var x = typeof(IEnumerable<object>).IsAssignableFrom(typeof(TestInput[]));

            ServiceLocator.ServiceProvider = new ServiceCollection()
                .AddCommix(c => c
                    .ModelPipelineFactory<ConsoleTestModelPiplineFactory>()
                    .PropertyPipelineFactory<ConsoleTestPropertyPipelineFactory>())
                .BuildServiceProvider();

            CommixExtensions.PipelineFactory =
                ServiceLocator.ServiceProvider.GetRequiredService<IModelPipelineFactory>();

            var results = new ConcurrentBag<TestOutput>();

            var threads = new List<Thread>();
            for (int i = 0; i < 100; i++)
            {
                var id = i;
                var thread = new Thread(() =>
                {
                    var input = new TestInput();

                    for (int xi = 0; xi < 100; xi++)
                    {
                       var output = input.As<TestOutput>();
                       results.Add(output);
                    }

                    Console.WriteLine($"{id} complete");
                });
                threads.Add(thread);

                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            foreach (TestOutput testOutput in results)
            {
                if (string.IsNullOrEmpty(testOutput.Name))
                    throw new NullReferenceException();
            }

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}