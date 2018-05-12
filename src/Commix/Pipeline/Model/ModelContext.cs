using System;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Model
{
    public class ModelContext
    {
        public object Input { get; }
        public object Output { get; }
        public ModelSchema Schema { get; set; }
        
        public ModelContext(object input, object output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
        }
    }
}