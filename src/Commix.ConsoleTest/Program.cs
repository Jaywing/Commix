using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Commix;
using Commix.Pipeline;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Schema.Extensions;

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

            var threads = new List<Thread>();

            for (int i = 0; i < 100; i++)
            {
                var id = i;
                var thread = new Thread(() =>
                {
                    for (int xi = 0; xi < 1000; xi++)
                    {
                        try
                        {
                            var input = new TestInput();
                            var output = input.As<TestOutput>();
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

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }

    public class UopModelMappingPipeline : ModelMappingPipeline
    {
        public UopModelMappingPipeline(SchemaGeneratorProcessor schemaGenerator, ModelMapperProcessor modelMapper)
        {
            Add(schemaGenerator, null);
            Add(modelMapper, null);
        }
    }

    public static class CommixRegistrar
    {
        public static IServiceCollection Commix(this IServiceCollection serviceCollection)
        {
            var commix = new CommixFactories();
            
            serviceCollection.AddSingleton<IModelPipelineFactory>(commix);
            serviceCollection.AddSingleton<IPropertyProcessorFactory>(commix);

            serviceCollection
                .RegisterProcessors("Commix");

            serviceCollection.AddSingleton<SchemaGeneratorProcessor, InMemorySchemaGeneratorProcessor>();
            serviceCollection.AddSingleton<ModelMapperProcessor>();
            serviceCollection.AddSingleton<ModelMappingPipeline, UopModelMappingPipeline>();

            CommixExtensions.PipelineFactory = commix;

            return serviceCollection;
        }

        public static IServiceCollection RegisterProcessors(this IServiceCollection serviceCollection, string assemblyPrefix)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith(assemblyPrefix));

            foreach (Assembly loadedAssembly in assemblies)
                RegisterProcessors(serviceCollection, loadedAssembly);

            return serviceCollection;
        }

        public static IServiceCollection RegisterProcessors(this IServiceCollection serviceCollection, Assembly assembly)
        {
            foreach (Type processorType in assembly.GetTypes())
            {
                switch (processorType)
                {
                    case var type when type.IsAbstract || type.IsInterface:
                        continue;
                    case var type when typeof(IPropertyProcesser).IsAssignableFrom(type):
                        serviceCollection.AddTransient(type);
                        break;
                }
            }

            return serviceCollection;
        }
    }

    public static class ServiceLocator
    {
        public static ServiceProvider ServiceProvider { get; set; }
    }

    public class CommixFactories : IModelPipelineFactory, IPropertyProcessorFactory
    {
        private readonly ConcurrentDictionary<Type, ModelMappingPipeline> _pipelines = new ConcurrentDictionary<Type, ModelMappingPipeline>();

        public ModelMappingPipeline GetPipeline(Type outputType)
        {
            return _pipelines.GetOrAdd(outputType, ServiceLocator.ServiceProvider.GetRequiredService<ModelMappingPipeline>());
        }

        public IPropertyProcesser GetProcessor(Type processorType)
            => (IPropertyProcesser)ServiceLocator.ServiceProvider.GetRequiredService(processorType);
    }

    public class PipelineDiagnosticsProcessor : IProcessor<ModelContext, ModelProcessorContext>
    {
        public Action Next { get; set; }
       
        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
        {
            var timer = new Stopwatch();
            timer.Start();
            Next();
            timer.Stop();
            Console.WriteLine($"Pipeline elapsed {timer.Elapsed.TotalMilliseconds}ms.");
        }
    }

    public class TestInput
    {
        public string Name { get; set; } = "Hello World";

        public TestInput2 Nested { get; set; } = new TestInput2();
    }

    public class TestInput2
    {
        public string Name { get; set; } = "Source Nested";
    }

    public class TestOutput : IFluentSchema
    {
        public string Name { get; set; }
        public string SomeDerivedString { get; set; }

        public TestOutput2 Nested { get; set; }

        public SchemaBuilder Map()
            => this.Schema(s => s
                .Property(m => m.Nested, c => c.NestedFrom())
                .Property(m => m.SomeDerivedString, c => c
                    .Get("Name")
                    .Add(Processor.Use<Processor1>()))
                .Property(m => m.Name, c => c.Get()));


        public class TestOutput2 : IFluentSchema
        {
            public string Name { get; set; }

            public SchemaBuilder Map()
                => this.Schema(s => s
                    .Property(m => m.Name, c => c.Get())
                );
        }
    }

    public class Processor1 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            pipelineContext.Value = $"Source was: '{pipelineContext.Value}'";

            Next();
        }
    }
}