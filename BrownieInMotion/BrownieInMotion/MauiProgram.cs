using Microsoft.Extensions.Logging;
using BrownieInMotion.Core.ViewModels;
using BrownieInMotion.Core.Services;
using BrownieInMotion.Converters;

namespace BrownieInMotion
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<SimulationViewModel>();
            builder.Services.AddSingleton<AnnualSimulationViewModel>();
            builder.Services.AddSingleton<BrownianMotionService>();
            builder.Services.AddSingleton<DecimalEntryConverter>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
