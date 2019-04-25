using System;
using System.Runtime.Serialization;
using Commix.Pipeline.Property;

namespace Commix.Exceptions
{
    public class InvalidContextException : ApplicationException
    {
        public InvalidContextException()
        {
        }

        public InvalidContextException(string message) : base(message)
        {
        }

        public InvalidContextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static InvalidContextException Create(PropertyContext pipelineContext)
            => new InvalidContextException($"Invalid context '{pipelineContext.Context.GetType()}'");
    }
}