using Splat;
using NesEmu.Avalonia.Extensions;
using NesEmu.Avalonia.ViewModels;
using NesEmu.Core;

namespace NesEmu.Avalonia.DependencyInjection
{
    public static class Bootstrapper
    {
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterLazySingleton<NintendoEntertainmentSystem>(() => new NintendoEntertainmentSystem());

            //View Models
            services.Register(() => new MainWindowViewModel(
                resolver.GetRequiredService<NintendoEntertainmentSystem>()
            ));
        }
    }
}