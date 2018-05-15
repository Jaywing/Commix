using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Commix.Core;
using Commix.Core.Diagnostics;
using Commix.Diagnostics;
using Commix.Diagnostics.Reactive;
using Commix.Pipeline.Model;
using Commix.Pipeline.Model.Processors;
using Commix.Pipeline.Property;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Commix.AspNetCore.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCommix()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            CommixExtensions.PipelineFactory = app.ApplicationServices.GetRequiredService<IModelPipelineFactory>();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    
    
    public class ThreadAwareLogger
    {
        private readonly ConcurrentDictionary<int, ConsolePipelineTrace> _threadTraces =
            new ConcurrentDictionary<int, ConsolePipelineTrace>();

        public void Attach(PipelineMonitor monitor)
        {
            _threadTraces.GetOrAdd(Thread.CurrentThread.ManagedThreadId,
                managedThreadId => new ConsolePipelineTrace(managedThreadId, monitor));
        }
    }

    public class ConsolePipelineTrace : ThreadedPipelineTrace
    {
        public ConsolePipelineTrace(int managedThreadId, PipelineMonitor monitor)
            : base(managedThreadId, monitor)
        { }

        protected override void OnRun(EventPattern<PipelineEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId}: Run");
        }

        protected override void OnComplete(EventPattern<PipelineEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId}: Complete");
        }

        protected override void OnError(EventPattern<PipelineErrorEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId} Error: {args.EventArgs.Error.Message}");
        }

        protected override void OnProcessorRun(EventPattern<PipelineProcessorEventArgs> args)
        {
            switch (args.EventArgs.PipelineContext)
            {
                case ModelContext modelContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Model:{args.EventArgs.ProcessorType}) Run");
                    break;
                case PropertyContext propertyContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Prop:{args.EventArgs.ProcessorType}) Run");
                    break;
            }
        }

        protected override void OnProcessorComplete(EventPattern<PipelineProcessorEventArgs> args)
        {
            switch (args.EventArgs.PipelineContext)
            {
                case ModelContext modelContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Model:{args.EventArgs.ProcessorType}) Complete");
                    break;
                case PropertyContext propertyContext:
                    Console.WriteLine($"{ManagedThreadId}: Processor(Prop:{args.EventArgs.ProcessorType}) Complete");
                    break;
            }
        }

        protected override void OnProcessorError(EventPattern<PipelineProcessorExceptionEventArgs> args)
        {
            Console.WriteLine($"{ManagedThreadId} Processor Error: {args.EventArgs.Error.Message}");
        }
    }
}
