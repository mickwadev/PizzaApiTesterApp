using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace PizzaApiTesterApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<TokenStorage>();
            builder.Services.AddTransientWithShellRoute<AuthTestsPage,AuthTestsPageViewModel>(nameof(AuthTestsPage));
            return builder.Build();
        }
    }
}
