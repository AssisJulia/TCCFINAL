using Blazored.SessionStorage;
using Microsoft.Extensions.Logging;
using Radzen;

namespace SAMMI.Presentation
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
                });

            builder.Services.AddMauiBlazorWebView();

            //builder.Services.AddScoped(httpClient => new HttpClient { BaseAddress = new Uri("https://localhost:7076") });
            builder.Services.AddScoped(httpClient => new HttpClient { BaseAddress = new Uri("https://sammigames-api.tccnapratica.com.br") });

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddBlazoredSessionStorage();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
