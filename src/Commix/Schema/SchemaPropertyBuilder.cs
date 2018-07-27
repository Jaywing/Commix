using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Commix.Schema
{
    public class SchemaPropertyBuilder
    {
        protected PropertyInfo PropertyInfo { get; set; }
        protected IList<ProcessorSchema> Processors { get; } = new List<ProcessorSchema>();

        public SchemaPropertyBuilder(PropertyInfo propertyInfo) => PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

        public void AddProcessorInfo(ProcessorSchema schema)
        {
            Processors.Add(schema);
        }

        public PropertyPipelineSchema Build() =>
            new PropertyPipelineSchema
            {
                PropertyInfo = PropertyInfo,
                Processors = Processors
            };
    }

    public class SchemaContextBuilder
    {
        protected IList<ProcessorSchema> Processors { get; } = new List<ProcessorSchema>();

        public void AddProcessorInfo(ProcessorSchema schema)
        {
            Processors.Add(schema);
        }

        public ContextProcessorSchema Build() =>
            new ContextProcessorSchema
            {
                Processors = Processors
            };
    }
    
    // ReSharper disable UnusedTypeParameter

    public class SchemaPropertyBuilder<TModel, TProp> : SchemaPropertyBuilder
    {
        public SchemaPropertyBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        { }
    }

    public class SchemaContextBuilder<TModel> : SchemaContextBuilder
    {
       
    }

    // ReSharper restore UnusedTypeParameter

    public static class SchemaPropertyBuilderExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Add<TModel, TProp, TProcessor>(
            this SchemaPropertyBuilder<TModel, TProp> builder, ProcesserDefinition<TProcessor> processor)
        {
            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }
    }

    public static class SchemaContextBuilderExtensions
    {
        public static SchemaContextBuilder<TModel> Add<TModel, TProcessor>(
            this SchemaContextBuilder<TModel> builder, ProcesserDefinition<TProcessor> processor)
        {
            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }
    }

    public static class Processor
    {
        public static ProcesserDefinition<T> Use<T>(
            Action<SchemaProcessorBuilder> configure = null)
                => new ProcesserDefinition<T>(configure);
    }

    public class ProcesserDefinition<TProcessor>
    {
        public SchemaProcessorBuilder SchemaBuilder { get; } 
            = new SchemaProcessorBuilder(typeof(TProcessor));

        public ProcesserDefinition(Action<SchemaProcessorBuilder> configure = null) 
            => configure?.Invoke(SchemaBuilder);
    }
}