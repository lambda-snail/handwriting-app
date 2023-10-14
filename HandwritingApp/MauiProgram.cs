using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using HandwritingApp.Data;
using HandwritingApp.Services;
using Microsoft.AspNetCore.Authorization;
using MudBlazor.Services;

namespace HandwritingApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMudServices();
        
        builder.Services.AddSingleton<WeatherForecastService>();
        
        builder.Services.AddSingleton<DefaultAzureCredential>(new DefaultAzureCredential());
        
        builder.Services.AddScoped<BlobServiceClient>(provider =>
        {
            var credential = provider.GetRequiredService<DefaultAzureCredential>();
            return new BlobServiceClient(
                new Uri("https://stdianalysis.blob.core.windows.net"),
                credential);
        });

        builder.Services.AddTransient<IDocumentAnalysisService, DocumentAnalysisService>();

        return builder.Build();
    }
}