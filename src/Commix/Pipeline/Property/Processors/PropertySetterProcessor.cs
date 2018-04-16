﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Schema;
using Commix.Tools;

namespace Commix.Pipeline.Property.Processors
{
    public class PropertySetterProcessor : IPropertyProcesser, IAsyncPropertyMappingProcesser
    {
        public Action Next { get; set; }
        
        public Func<Task> NextAsync { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            FastPropertyAccessor.SetValue(pipelineContext.PropertyInfo, pipelineContext.ModelContext.Output, pipelineContext.Value);
            
            Next();
        }

        public Task Run(PropertyContext context, CancellationToken cancellationToken)
        {
            FastPropertyAccessor.SetValue(context.PropertyInfo, context.ModelContext.Output, context.Value);

            return NextAsync();
        }
    }
}