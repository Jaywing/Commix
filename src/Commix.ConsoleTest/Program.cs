using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

    public class UopModelMappingPipeline : ModelMappingPipeline
    {
        public UopModelMappingPipeline(SchemaGeneratorProcessor schemaGenerator, IModelMapperProcessor modelMapper)
        {
            Add(schemaGenerator, new ModelProcessorContext());
            Add(modelMapper, new ModelProcessorContext());
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

            var modelMapperProcessorFactory = new Func<IServiceProvider, IModelMapperProcessor>(provider =>
            {
                var modelMapper = provider.GetRequiredService<ModelMapperProcessor>();
                return new LoggingModelMapperProcessor(modelMapper);
            });

            serviceCollection.AddTransient<SchemaGeneratorProcessor, InMemorySchemaGeneratorProcessor>();

            serviceCollection.AddSingleton<ModelMapperProcessor>();
            serviceCollection.AddSingleton(modelMapperProcessorFactory);
            
            serviceCollection.AddTransient<ModelMappingPipeline, UopModelMappingPipeline>();

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

    public class LoggingModelMapperProcessor : IModelMapperProcessor
    {
        private readonly ConcurrentDictionary<int, ThreadModelMapTrace> _threadTraces =
            new ConcurrentDictionary<int, ThreadModelMapTrace>();

        private readonly IModelMapperProcessor _processor;

        public LoggingModelMapperProcessor(IModelMapperProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public Action Next
        {
            get => _processor.Next;
            set => _processor.Next = value;
        }

        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
        {
            if (_processor is IObservableModelMapperProcessor observableProcessor)
            {
                _threadTraces.GetOrAdd(Thread.CurrentThread.ManagedThreadId,
                    managedThreadId => new ThreadModelMapTrace(managedThreadId, observableProcessor));
            }

            _processor.Run(pipelineContext, processorContext);
        }
    }

    public class ThreadModelMapTrace : IDisposable
    {
        public int ManagedThreadId { get; }

        private readonly IDisposable _onRun;
        private readonly IDisposable _onComplete;

        private readonly Stack<Guid> _instanceStack = new Stack<Guid>();

        public ThreadModelMapTrace(int managedThreadId, IObservableModelMapperProcessor observableProcessor)
        {
            ManagedThreadId = managedThreadId;

            var onRun = Observable.FromEventPattern<ModelMapperMonitor.ModelMapperMonitorArgs>(
                h => observableProcessor.Monitor.RunEvent += h,
                h => observableProcessor.Monitor.RunEvent -= h);

            _onRun = onRun
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnNext);

            var onComplete = Observable.FromEventPattern<ModelMapperMonitor.ModelMapperMonitorArgs>(
                h => observableProcessor.Monitor.CompleteEvent += h,
                h => observableProcessor.Monitor.CompleteEvent -= h);

            _onComplete = onComplete
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnComplete);
        }

        private void OnNext(EventPattern<ModelMapperMonitor.ModelMapperMonitorArgs> eventPattern)
        {
            //Console.WriteLine($"{string.Concat(Enumerable.Repeat('>', _instanceStack.Count))} {ManagedThreadId}-{eventPattern.EventArgs.PipelineContext.InstanceId}(Run): {eventPattern.EventArgs.PipelineContext.Schema.ModelType}");
            Console.WriteLine($"{string.Concat(Enumerable.Repeat('>', _instanceStack.Count))} {ManagedThreadId} {eventPattern.EventArgs.ModelType} (Run)");

            _instanceStack.Push(Guid.NewGuid());
        }

        private void OnComplete(EventPattern<ModelMapperMonitor.ModelMapperMonitorArgs> eventPattern)
        {
             _instanceStack.Pop();

            Console.WriteLine($"{string.Concat(Enumerable.Repeat('>', _instanceStack.Count))} {ManagedThreadId} {eventPattern.EventArgs.ModelType} (Complete)");
            //Console.WriteLine($"{string.Concat(Enumerable.Repeat('>', _instanceStack.Count))} {ManagedThreadId}-{eventPattern.EventArgs.PipelineContext.InstanceId}(Complete):{eventPattern.EventArgs.PipelineContext.Schema.ModelType}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onRun?.Dispose();
                _onComplete?.Dispose();
            }
        }
    }

    public static class ServiceLocator
    {
        public static ServiceProvider ServiceProvider { get; set; }
    }

    public class CommixFactories : IModelPipelineFactory, IPropertyProcessorFactory
    {
        private readonly ConcurrentDictionary<(int ThreadId, Type modelType), ModelMappingPipeline> _pipelines = new ConcurrentDictionary<(int ThreadId, Type modelType), ModelMappingPipeline>();

        public ModelMappingPipeline GetPipeline(Type outputType)
        {
            return (ModelMappingPipeline)ServiceLocator.ServiceProvider.GetRequiredService<ModelMappingPipeline>();
            //return _pipelines.GetOrAdd((Thread.CurrentThread.ManagedThreadId, outputType), ServiceLocator.ServiceProvider.GetRequiredService<ModelMappingPipeline>());
        }

        public IPropertyProcesser GetProcessor(Type processorType)
        {
            return (IPropertyProcesser) ServiceLocator.ServiceProvider.GetRequiredService(processorType);
        }
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

    public class Processor1 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            pipelineContext.Value = $"Source was: '{pipelineContext.Value}'";

            Next();
        }
    }

    public class Processor2 : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            pipelineContext.Value = new TestOutput3().As<TestOutput3>();

            Next();
        }
    }
}