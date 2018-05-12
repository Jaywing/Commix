using System;
using Commix.Schema;
using Commix.Schema.Extensions;

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
                    .ConstantValue("Welcome to Commix!").Set()
                )
            );
    }
}