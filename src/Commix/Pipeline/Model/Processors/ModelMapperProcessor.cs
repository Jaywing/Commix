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
            if (pipelineContext.Schema != null)
            {
                Monitor.OnRunEvent(new ModelMapperMonitor.ModelMapperMonitorArgs(pipelineContext.Schema.ModelType));

                foreach (var propertySchema in pipelineContext.Schema.Properties)
                {
                    RunPropertyPipeline(pipelineContext, propertySchema);
                }

                Monitor.OnCompleteEvent(new ModelMapperMonitor.ModelMapperMonitorArgs(pipelineContext.Schema.ModelType));
            }

            Next();
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
            public Type ModelType { get; }

            public ModelMapperMonitorArgs(Type modelType)
            {
                ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
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