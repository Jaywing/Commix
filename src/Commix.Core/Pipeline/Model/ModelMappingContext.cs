using System;

using Commix.Core.Schema;

namespace Commix.Core.Pipeline.Model
{
    public class ModelMappingContext<T>
    {
        public object Input { get; }
        public T Output { get; set; }
        
        public ModelSchema Schema { get; set; }

        public ModelMappingContext(object input)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
        }
    }
}