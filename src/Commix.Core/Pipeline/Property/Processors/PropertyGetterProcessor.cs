using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Commix.Core.Tools;

namespace Commix.Core.Pipeline.Property.Processors
{
    public class PropertyGetterProcessor<TModel> : IPropertyMappingProcesser<TModel>,  IAsyncPropertyMappingProcesser<TModel>
    {
        public static string SourcePropertyOption = $"{typeof(PropertyGetterProcessor<TModel>).Name}.SourcePropertyOption"; 
        
        public Dictionary<string, object> Options { get; set; }

        public Action Next { get; set; }
        public Func<Task> NextAsync { get; set; }

        public void Run(PropertyMappingContext<TModel> context)
        {
            PropertyInfo sourcePropertyInfo = GetPropertyInfo(context);
            if (sourcePropertyInfo != null)
            {
                context.Value = FastPropertyAccessor.GetValue(sourcePropertyInfo, context.ModelMappingContext.Input);
                Next();
            }
        }
        
        public async Task Run(PropertyMappingContext<TModel> context, CancellationToken cancellationToken)
        {
            PropertyInfo sourcePropertyInfo = GetPropertyInfo(context);
            if (sourcePropertyInfo != null)
            {
                context.Value = FastPropertyAccessor.GetValue(sourcePropertyInfo, context.ModelMappingContext.Input);
                await NextAsync();
            }
        }

        private PropertyInfo GetPropertyInfo(PropertyMappingContext<TModel> context)
        {
            var sourceProperty = context.PropertyInfo.Name;
            if (Options.ContainsKey(SourcePropertyOption))
                sourceProperty = (string) Options[SourcePropertyOption];

            var sourcePropertyInfo = context.ModelMappingContext.Input.GetType().GetProperty(sourceProperty);
            return sourcePropertyInfo;
        }

    }
}