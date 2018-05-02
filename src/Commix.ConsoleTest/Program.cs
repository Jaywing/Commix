using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commix;
using Commix.ConsoleTest.Models;
using Commix.Pipeline;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceLocator.ServiceProvider = new ServiceCollection()
                .Commix()
                .BuildServiceProvider();

            var results = new ConcurrentBag<TestOutput>();

            var threads = new List<Thread>();
            for (int i = 0; i < 50; i++)
            {
                var id = i;
                var thread = new Thread(() =>
                {
                    var input = new TestInput();

                    for (int xi = 0; xi < 50; xi++)
                    {
                        try
                        {
                            var output = input.As<TestOutput>();

                            results.Add(output);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }

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