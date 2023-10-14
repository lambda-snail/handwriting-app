using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using HandwritingApp.Services;
using MudBlazor.Services;
using OneOf;

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

        builder.Services.AddSingleton<BlobClientFactory>();

        builder.Services.AddScoped<IDocumentAnalysisService, DocumentAnalysisService>();

        return builder.Build();
    }
}