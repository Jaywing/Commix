using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Commix.ConsoleTest
{
    public static class ServiceLocator
    {
        public static ServiceProvider ServiceProvider { get; set; }
    }
}