﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


using Commix.Pipeline.Property;
using Commix.Schema;
using Commix.Sitecore.Processors;
using Commix.Sitecore.Tools;


namespace Commix.Sitecore.Processors
{
    /// <summary>
    /// Set the model target property value using the current context, unless the property pipeline is flagged as faulted.
    /// </summary>
    public class SetProcessor : IPropertyProcessor
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                    FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.MappingContext.Output, pipelineContext.Context);
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }
            finally
            {
                Next();
            }
        }

        public PropertyStageMarker AllowedStages { get; } = PropertyStageMarker.All;
    }

    

    /// <summary>
    /// Switch context to the value of a property on the model pipeline source.
    /// </summary>
    public class GetProcessor : IPropertyProcessor
    {
        public static string SourcePropertyOptionKey = $"{typeof(GetProcessor).Name}.SourcePropertyOptionKey"; 
        
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted && GetPropertyInfo(pipelineContext, processorContext, out PropertyInfo sourcePropertyInfo))
                {
                    pipelineContext.Context = FastPropertyAccessor.GetValue(sourcePropertyInfo, pipelineContext.MappingContext.Input);
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }
            finally
            {
                Next();
            }
        }

        private bool GetPropertyInfo(PropertyContext context, ProcessorSchema processorContext, out PropertyInfo sourcePropertyInfo)
        {
            if (!processorContext.TryGetOption(SourcePropertyOptionKey, out string sourceProperty))
            {
                sourceProperty = context.PropertyInfo.Name;
            }

            sourcePropertyInfo = context.MappingContext.Input.GetType().GetProperty(sourceProperty);

            return sourcePropertyInfo != null;
        }
    }
}

// ReSharper disable once CheckNamespace
namespace Commix.Schema
{
    public static class SetStageProcessorExtensions
    {
        public static SchemaPropertyBuilder<TModel, TProp> Set<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<SetProcessor>(configure));
        }
    }

    public static class GetProcessorExtensions
    {
        /// <summary>
        /// Set the context from a property on the source with the same property name as the target, i.e. straight through mapping.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Get<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, 
            Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<GetProcessor>(configure));
        }

        /// <summary>
        /// Switch the context to a property on the source with the given alias.
        /// </summary>
        /// <typeparam name="TModel">The target model.</typeparam>
        /// <typeparam name="TProp">The target property.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="sourceProperty">The name of property on the source.</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static SchemaPropertyBuilder<TModel, TProp> Get<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder, string sourceProperty, 
            Action<SchemaProcessorBuilder> configure = null)
        {
            return builder
                .Add(Processor.Property<GetProcessor>(c =>
                {
                    c.Option(GetProcessor.SourcePropertyOptionKey, sourceProperty);
                    configure?.Invoke(c);
                }));
        }
    }
}
