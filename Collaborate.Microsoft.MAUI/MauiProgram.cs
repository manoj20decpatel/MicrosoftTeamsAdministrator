using AuthServices;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using MauiAuth;

namespace Collaborate.Microsoft.MAUI;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.Services.AddScoped<MainPage>();
        builder.Services.AddScoped<AzureB2CPage>();
        builder.Services.AddScoped<AzureADPage>();
        builder.Services.RegisterServices();
        builder.UseMauiCommunityToolkit();
        return builder.Build();
	}
}
