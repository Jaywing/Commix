using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Commix.Pipeline.Property;
using Commix.Schema;

namespace Commix.Pipeline.Model.Processors
{
    public interface IModelMapperProcessor : IProcessor<ModelContext, ModelProcessorContext>
    {
        
    }

    public interface IObservableModelMapperProcessor
    {
        ModelMapperMonitor Monitor { get; }
    }

    public class ModelMapperProcessor : IModelMapperProcessor, IObservableModelMapperProcessor
    {
        private readonly IPropertyProcessorFactory _processorFactory;

        public Action Next { get; set; }

        public ModelMapperMonitor Monitor { get; } = new ModelMapperMonitor();

        public ModelMapperProcessor(IPropertyProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory ?? throw new ArgumentNullException(nameof(processorFactory));
        }

        public void Run(ModelContext pipelineContext, ModelProcessorContext processorContext)
        {
            Monitor.OnRunEvent(new ModelMapperMonitor.ModelMapperMonitorArgs(this, pipelineContext, processorContext));

            if (pipelineContext.Schema != null)
            {
                foreach (var propertySchema in pipelineContext.Schema.Properties)
                {
                    RunPropertyPipeline(pipelineContext, propertySchema);
                }
            }

            Next();

            Monitor.OnCompleteEvent(new ModelMapperMonitor.ModelMapperMonitorArgs(this, pipelineContext, processorContext));
        }

        private void RunPropertyPipeline(ModelContext context, PropertySchema propertySchema)
        {
            var propertyPipeline = new PropertyMappingPipeline();
            foreach (PropertyProcessorSchema propertyProcessorSchema in propertySchema.Processors)
            {
                var processor = _processorFactory.GetProcessor(propertyProcessorSchema.Type);
                if (processor is IPropertyProcesser syncProcessor)
                {
                    propertyPipeline.Add(syncProcessor, propertyProcessorSchema);
                }
            }

            propertyPipeline.Run(CreatePropertyContext(context, propertySchema));
        }

        private PropertyContext CreatePropertyContext(ModelContext context, PropertySchema propertySchema)
            => new PropertyContext(context, propertySchema.PropertyInfo, context.Input);
    }

    public class ModelMapperMonitor
    {
        public event EventHandler<ModelMapperMonitorArgs> RunEvent;
        public event EventHandler<ModelMapperMonitorArgs> CompleteEvent;
        
        public class ModelMapperMonitorArgs
        {
            public DateTime Timestamp { get; } = DateTime.Now;
            public IObservableModelMapperProcessor Processor { get; }
            public ModelContext PipelineContext { get; }
            public ModelProcessorContext ProcessorContext { get; }

            public ModelMapperMonitorArgs(IObservableModelMapperProcessor processor, ModelContext pipelineContext,
                ModelProcessorContext processorContext)
            {
                Processor = processor ?? throw new ArgumentNullException(nameof(processor));
                PipelineContext = pipelineContext ?? throw new ArgumentNullException(nameof(pipelineContext));
                ProcessorContext = processorContext;
            }
        }

        public void OnRunEvent(ModelMapperMonitorArgs e)
        {
            RunEvent?.Invoke(this, e);
        }

        public virtual void OnCompleteEvent(ModelMapperMonitorArgs e)
        {
            CompleteEvent?.Invoke(this, e);
        }
    }
}