using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Commix.ConsoleTest.Models;
using Commix.ConsoleTest.Tools;
using Commix.Diagnostics;
using Commix.Pipeline.Mapping;
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
            
            ServiceLocator.ServiceProvider = new ServiceCollection()
                .AddCommix(c => c
                    .MappingPipelineFactory<ConsoleTestModelPiplineFactory>()
                    .PropertyPipelineFactory<ConsoleTestPropertyPipelineFactory>())
                .BuildServiceProvider();

            CommixExtensions.PipelineFactory = new Lazy<IMappingPipelineFactory>(
                () => ServiceLocator.ServiceProvider.GetRequiredService<IMappingPipelineFactory>());

            var monitor = new PipelineMonitor();
            var consoleTrace = new ThreadAwareLogger();

            CommixExtensions.GlobalPipelineConfig = (pipeline, context) =>
            {
                context.Monitor = monitor;

                consoleTrace.Attach(context.Monitor);
            };

            var results = new ConcurrentBag<TestOutput>();

            var threads = new List<Thread>();
            for (int i = 0; i < 1; i++)
            {
                var id = i;
                var thread = new Thread(() =>
                {
                    var input = new TestInput();

                    for (int xi = 0; xi < 1; xi++)
                    {
                        var jsonTrace = new NestedPipelineTrace(Thread.CurrentThread.ManagedThreadId);

                        var output = input.As<TestOutput>((pipeline, context) =>
                        {
                            jsonTrace.Attach(context.Monitor);
                        });

                        //string json = jsonTrace.ToJson();

                        //File.WriteAllText($"Trace_{id}_{xi}.json", json, Encoding.UTF8);

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

            foreach (var testOutput in results)
            {
                if (string.IsNullOrEmpty(testOutput.Name))
                    throw new NullReferenceException();
            }

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}