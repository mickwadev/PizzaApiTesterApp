using ApiService;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Refit;

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
            string url = "https://localhost:7144";
            
          //  builder.Services.AddTransient<AuthHeaderHandler>();
            builder.Services.AddHttpClient("refresh", c => c.BaseAddress = new Uri(url));

            builder.Services.AddRefitClient<IPizzaApiv1>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(url))
            ;//    .AddHttpMessageHandler<AuthHeaderHandler>();

            return builder.Build();
        }
    }
}
