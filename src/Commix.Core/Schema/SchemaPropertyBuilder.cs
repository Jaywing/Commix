using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Commix.Core.Pipeline.Property;

namespace Commix.Core.Schema
{
    public class SchemaPropertyBuilder
    {
        protected PropertyInfo PropertyInfo { get; set; }
        protected IList<PropertyProcessorSchema> Processors { get; } = new List<PropertyProcessorSchema>();

        public SchemaPropertyBuilder(PropertyInfo propertyInfo) => PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

        public PropertySchema Build() =>
            new PropertySchema
            {
                PropertyInfo = PropertyInfo,
                Processors = Processors
            };
    }
    
    public class SchemaPropertyBuilder<TModel, TProp> : SchemaPropertyBuilder
    {
        public SchemaPropertyBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        { }

        public void AddProcessorInfo(PropertyProcessorSchema schema)
        {
            Processors.Add(schema);
        }
    }

    public static class SchemaPropertyBuilderExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Add<TModel, TProp, TProcessor>(
            this SchemaPropertyBuilder<TModel, TProp> builder, ProcesserDefinition<TProcessor> processor)
        {
            builder.AddProcessorInfo(processor.SchemaBuilder.Build());

            return builder;
        }
    }

    public static class Processor
    {
        public static ProcesserDefinition<T> Use<T>(
            Action<SchemaPropertyProcessorBuilder> configure = null)
                => new ProcesserDefinition<T>(configure);
    }

    public class ProcesserDefinition<TProcessor>
    {
        public SchemaPropertyProcessorBuilder SchemaBuilder { get; } 
            = new SchemaPropertyProcessorBuilder(typeof(TProcessor));

        public ProcesserDefinition(Action<SchemaPropertyProcessorBuilder> configure = null)
        {
            configure?.Invoke(SchemaBuilder);
        }
    }

    public static class ProcessorDefinitionExtensions
    {
        //public static ProcesserDefinition<TProcessor> Configure<TProcessor>(this ProcesserDefinition<TProcessor> definition, Action<SchemaPropertyProcessorBuilder> configure)
        //{
        //    configure(definition.SchemaBuilder);

        //    return definition;
        //}
    }
}