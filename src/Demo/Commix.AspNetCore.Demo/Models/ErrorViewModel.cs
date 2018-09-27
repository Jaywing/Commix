using System;

using Commix.Core.Schema;
using Commix.Pipeline.Property.Processors;
using Commix.Schema;

namespace Commix.AspNetCore.Demo.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class HomeIndexViewModel : IFluentSchema
    {
        public string Heading { get; set; }

        public SchemaBuilder Map()
            => this.Schema(s => s
                .Property(m => m.Heading, p => p
                    .Constant("Welcome to Commix!")
                    .Set()
                )
            );
    }
}