using Splat;
using System;

namespace NesEmu.Avalonia.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        {
            var service = resolver.GetService<TService>();
            if (service is null)
            {
                throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}");
            }

            return service;
        }
    }
}