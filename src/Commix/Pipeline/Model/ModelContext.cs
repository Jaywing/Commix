using System;
using System.Linq;

using Commix.Schema;

namespace Commix.Pipeline.Model
{
    public class ModelContext
    {
        public object Input { get; }
        public object Output { get; set; }
        public ModelSchema Schema { get; set; }
        public Guid InstanceId { get; } = Guid.NewGuid();
        public Guid ParentId { get; set; }
        
        public ModelContext(object input, object output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
        }
    }
}