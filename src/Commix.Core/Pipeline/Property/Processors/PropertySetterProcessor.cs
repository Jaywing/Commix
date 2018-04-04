using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commix.Core.Tools;

namespace Commix.Core.Pipeline.Property.Processors
{
    public class PropertySetterProcessor<T> : IPropertyMappingProcesser<T>, IAsyncPropertyMappingProcesser<T>
    {
        public Dictionary<string, object> Options { get; set; }
        
        public Action Next { get; set; }
        public Func<Task> NextAsync { get; set; }

        public void Run(PropertyMappingContext<T> context)
        {
            FastPropertyAccessor.SetValue(context.PropertyInfo, context.ModelMappingContext.Output, context.Value);

            Next();
        }

        public Task Run(PropertyMappingContext<T> context, CancellationToken cancellationToken)
        {
            FastPropertyAccessor.SetValue(context.PropertyInfo, context.ModelMappingContext.Output, context.Value);

            return NextAsync();
        }
    }
}